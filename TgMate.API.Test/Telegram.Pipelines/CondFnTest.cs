using TgMate.API.Telegram;
using TgMate.API.Telegram.Pipelines.FnMembers;
using TgMate.API.Telegram.Pipelines.Fns.Flow;
using TgMate.API.Telegram.Pipelines.Fns.Messages;

namespace TgMate.API.Test.Telegram.Pipelines;

public class CondFnTest
{
  [Fact]
  public async Task Check()
  {
    var message = new Message { Value = new TelegramMessageDTO(Uid: 1, Text: "hello") };

    {
      var condFn = new CondFn()
      {
        Input =
        [
          new HasTextFn { Input = [message, new Str() { Value = "hello" }] },
          new InputFn() { Input = [new UnitReference() { Key = "unit key" }] },
          new InputFn() { Input = [new Int() { Value = -1 }] },
        ],
      };

      Assert.True(await condFn.Run() is [UnitReference { Key: string { } }]);
    }

    {
      var condFn = new CondFn()
      {
        Input =
        [
          new HasTextFn { Input = [message, new Str() { Value = "no" }] },
          new InputFn() { Input = [new Str() { Value = "yes" }] },
          new InputFn() { Input = [new Int() { Value = -1 }] },
        ],
      };

      Assert.True(await condFn.Run() is [Int { Value: -1 }]);
    }
  }
}
