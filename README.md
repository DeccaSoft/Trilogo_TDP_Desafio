# Trílogo_TDP_Desafio
## Turma TDP Trílogo BackEnd C#

Repositório do Desafio de BackEnd do Trílogo Development Program (TDP) 2021.
Essa é a Primeira turma (Ediçào Piloto) do programa construído para capacitar profissionais que não atuam na área do desenvolvimento, mas que teem interesse em aprender, seja por hobby ou pr interesse profissional.
Com foco em Back-End em C#, foram 8 sábados de muito conteúdo (De Lógica de Programação), passando por a construção de API's e finalizando com conceitos de Banco de Dados.

Ao término do curso, foi lançado um desafio para os alunos...

Segue o link do desafio:
[https://github.com/joaobatist4/trilogo-desafio-backend]

## Em resumo:

O Desafio é a construção de uma API de Ordens de Vendas de Produtos em C# com o Cadastros dos mesmos, bem como dos usuários e dos pedidos, culminando com um relatório final resumimdo.

E essa foi a minha Solução... espero que gostem!

Na pasta SQL encontram-se uma cópia da Base de Dados e os Scripts de Craição do Banco (Tanto utilizando o Migrations bem como o prório MySQL Workbench)

Tem também um arquivo Desafio TDP - Testes - CheckList.xlsx que é uma planiha com o estudo de caso que utilizei para testar minha api.

Para utilizar autenticação (JWT) e autorizações com o Swagger, siga os seguintes passos:
* Vá em Authentication (Post) e clique em "Try it out"
* Apague a linha "id": 0,     (Inclusive, sempre que for criar (POST) qualquer objeto, Apague essa linha)
* e substitua os campos 'string' para focar igual abaixo:
```
{
  "name": "andre",
  "email": "a@a.com",
  "password": "andre"
}
``` 
* Clique em Execute
E aparecerá como retorno (em Response body) uma chave como essa:
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFuZHJlIiwiZW1haWwiOiJhQGEuY29tIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNjQzMzA3NjIyLCJleHAiOjE2NDMzMTEyMjIsImlhdCI6MTY0MzMwNzYyMn0.QgxPbYnwFXc9GDSxtVkSEPpCr14LgPpsouNOJUs-HtQ
```
* Copie essa chave
* Lá em cima do lado direito clique em 'Authorize' e aparecerá a janela 'Avaliable authorizations'
* Na caixa 'Value:' digite 'Bearer', seguido de 'espaço' e, depois, a chave, como a seguir:
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFuZHJlIiwiZW1haWwiOiJhQGEuY29tIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNjQzMzA3NjIyLCJleHAiOjE2NDMzMTEyMjIsImlhdCI6MTY0MzMwNzYyMn0.QgxPbYnwFXc9GDSxtVkSEPpCr14LgPpsouNOJUs-HtQ
```
* Clique em Authorize
* Clique em Close

Pronto, agora é só brincar... (hehehe)




## Observação:
Ainda pretendo melhorar, aperfeiçoar, aplicar Clean Code, SOLID e muito mais durante meu aprendizado, portanto, estou aberto para dicas e sugestões, obrigado!

