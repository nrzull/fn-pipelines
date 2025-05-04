set dotenv-load

@api *args:
  cd TgMate.API && {{args}}

@api-test *args:
  cd TgMate.API.Test && {{args}}

@frontend *args:
  cd frontend && {{args}}

@fdev:
  just frontend npm run dev

@bdev:
  just api dotnet run

@setup:
  just api dotnet restore
  just api-test dotnet restore
  just frontend npm ci

@build:
  just api dotnet build
  just api-test dotnet build
  just frontend npm run build

@test:
  just api-test dotnet test

@devops:
  docker compose up -d
