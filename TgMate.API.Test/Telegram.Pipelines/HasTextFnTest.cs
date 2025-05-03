using TgMate.API.Telegram;
using TgMate.API.Telegram.Pipelines;
using TgMate.API.Telegram.Pipelines.FnMembers;
using TgMate.API.Telegram.Pipelines.Fns.Messages;

namespace TgMate.API.Test.Telegram.Pipelines;

public class HasTextFnTest
{
  [Fact]
  public async Task ShouldHaveHello()
  {
    var message = new Message() { Value = new TgMessageDTO(Uid: 1, Text: "hello") };

    var hasTextFn = new HasTextFn() { Input = [message, new Str() { Value = "hello" }] };

    Assert.True(await hasTextFn.Run() is [Bool { Value: true }]);
  }

  [Fact]
  public async Task ShouldThrow()
  {
    var message = new Message() { Value = new TgMessageDTO(Uid: 1, Text: "hello") };

    await Assert.ThrowsAsync<FnMembersException>(new HasTextFn() { Input = [message] }.Run);

    await Assert.ThrowsAsync<FnMembersException>(
      new HasTextFn() { Input = [message, new Bool()] }.Run
    );
  }
}
