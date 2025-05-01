using System.Text.Json;
using TgMate.API.Telegram.Pipelines;
using TgMate.API.Telegram.Pipelines.FnMembers;
using TgMate.API.Telegram.Pipelines.Fns.Flow;

namespace TgMate.API.Test.Telegram.Pipelines;

public class FnMemberJsonSerializerContextTest
{
  [Fact]
  public void CheckFn()
  {
    var fn = new InputFn() { Input = [new Str { Value = "hello" }] };
    var serFn = JsonSerializer.SerializeToElement(fn, FnMemberJsonSerializerContext.Default.Fn);

    Assert.True(
      JsonSerializer.Deserialize(serFn, FnMemberJsonSerializerContext.Default.Fn)
        is InputFn { Input: [Str { Value: "hello" }] }
    );
  }

  [Fact]
  public void CheckInput()
  {
    List<FnMember> input = [new UnitReference() { Key = "someref" }];
    var serInput = JsonSerializer.SerializeToElement(
      input,
      FnMemberJsonSerializerContext.Default.ListFnMember
    );

    Assert.True(
      JsonSerializer.Deserialize(serInput, FnMemberJsonSerializerContext.Default.ListFnMember)
        is [UnitReference { Key: "someref" }]
    );
  }
}
