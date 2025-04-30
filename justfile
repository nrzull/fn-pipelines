set dotenv-load


@api *args:
  cd TgMate.API && {{args}}

@test:
  cd TgMate.API.Test && dotnet test
