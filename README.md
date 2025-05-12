Passo a passo execução da aplicação:

Ambiente local:
- A aplicação pode ser executada normalmente pelo Visual Studio ou outros
- A aplicação pode ser executada via docker compose.
  Buildar a imagem: docker build -f Stefanini.Api/Dockerfile -t nome-imagem .
  Subir o compose:  docker compose --project-name nome up
  Após isso basta acessar http://localhost:5000/swagger

Ambiente de produção:
- A aplicação esta rodando via docker compose em uma EC2 na AWS.
- O processo de deploy é feito via github actions, basta subir algo na branch master.

- Endereço da aplicação: https://backend.financesapi.online/swagger

O projeto esta com 81% de cobertura de teste.
