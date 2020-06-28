# Simple Chat
Serviço de bate papo desenvolvido utilizando linguagem C#.NET.

## Definição
Um serviço de bate papo consiste em um servidor que permite a conexão de vários clientes simultaneamente para troca de mensagens. Ao se conectar, o usuário escolhe um apelido único  para depois entrar em uma sala, onde poderá conversar com outros usuários que estejam conectados.

## Requisitos
- .NET Core SDK 3.1

## Estrutura
Aplicação consiste em 2 projetos, sendo:
- Servidor
- Cliente

## Executando
Antes, é necessário compilar a solução para que as aplicações sejam geradas. Para compilar o projeto, com os requisitos instalados, via terminal:

Abra a pasta do projeto, e na raiz da solução entre com o seguinte comando:
```bash
dotnet build -c Release src
```

Esse comando irá compilar o projeto em Release e gerar os arquivos em: 
* `./src/Server/bin/Release/netcoreapp3.1/Server.exe`
* `./src/Client/bin/Release/netcoreapp3.1/Client.exe`

A aplicação utiliza por padrão a porta `8888` tanto no servidor quanto no cliente para realizar a comunicação, se desejar alterar a porta, basta passar a porta como argumento na chamada da aplicação:

```bash
Server.exe <porta>
Client.exe <porta>
```

## Comandos
Após entrada com o nome do usuário ao chat:
> `/help`: Exibir lista de comandos

> `/p @<nickname>`: Enviar mensagem privada para o usuário ***nickname***

> `@<nickname>`: Enviar mensagem pública para o usuário ***nickname***

> `/exit`: Sair da aplicação

> Obs: Os demais comandos ainda não foram implementados.