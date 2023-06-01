## Controle de Ponto API
#### Sobre esta solu��o:
- Esta solu��o foi desenvolvida por **Werner Moecke** para o desafio �lia.
- A solu��o � composta de 2 projetos:
    - **ControlePontoAPI:** Este � o projeto principal, contendo as Controllers, objetos e demais itens desenvolvidos.
    - **ControlePontoAPI.UnitTests:** Cont�m todos os testes unit�rios para o projeto principal.

#### Detalhes de implementa��o:
- Esta solu��o foi desenvolvida com o *target framework* **_.NET 6.0_**.
- Tamb�m foi optado por incluir a gera��o da documenta��o *OpenAPI v3.0* atrav�s do componente **_Swagger_**.
    - Uma vez que o projeto for executado no Visual Studio em modo **_Debug_**, a documenta��o poder� ser visualizada no browser:
        - *https://localhost:7000/swagger*, para conex�o segura
        - *http://localhost:5000/swagger*, para conex�o insegura
- Os 2 m�todos expostos pela API poder�o ser chamados pelo pr�prio *Swagger* (atrav�s de sua p�gina aberta no navegador), ou tamb�m pelos seguintes endpoints:
    - *http[s]://localhost:[5000|7000]/v1/batidas*, passando no corpo da requisi��o, um objeto v�lido do tipo **_Momento_** (vide detalhes na documenta��o);
    - *http[s]://localhost:[5000|7000]/v1/folhas-de-ponto*, passando na *query* o valor do m�s e ano no formato **_"yyyy-MM"_** (vide detalhes na documenta��o).