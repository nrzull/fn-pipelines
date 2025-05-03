using Microsoft.EntityFrameworkCore;
using TgMate.API.Core;

namespace TgMate.API.Telegram;

public class TgContext(ApplicationDbContext dbContext, TgWorkerClientPool tgWorkerClientPool)
{
  public async Task<TgWorker?> GetTgWorker(Guid id) =>
    await dbContext.TgWorkers.Where(v => v.Id == id).FirstOrDefaultAsync();

  public async Task<TgWorker?> GetTgWorker(Phone phone) =>
    await dbContext.TgWorkers.Where(v => v.Phone == phone.Value).FirstOrDefaultAsync();

  public async Task<TgWorker> EnsureTgWorker(Phone phone)
  {
    if (await GetTgWorker(phone) is TgWorker existing)
    {
      return existing;
    }

    var tgWorker = new TgWorker() { Phone = phone.Value, CreatedAt = DateTimeOffset.UtcNow };

    dbContext.Add(tgWorker);
    await dbContext.SaveChangesAsync();

    return tgWorker;
  }

  public async Task<WTelegram.Client> EnsureTgWorkerClient(
    string phone,
    Func<Task<string?>> getVerificationCode,
    bool forceLogin = true
  ) => await tgWorkerClientPool.Ensure(phone, getVerificationCode, forceLogin);

  public async Task DestroyTgWorkerClient(string phone) => await tgWorkerClientPool.Destroy(phone);

  public WTelegram.Client? GetTgWorkerClient(string phone) => tgWorkerClientPool.Get(phone);
}
