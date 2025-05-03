set dotenv-load

@api *args:
  cd TgMate.API && {{args}}

@api-test *args:
  cd TgMate.API.Test && {{args}}

@check:
  just api dotnet build
  just api-test dotnet build

@test:
  just api-test dotnet test
