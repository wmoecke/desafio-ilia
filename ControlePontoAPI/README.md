## Controle de Ponto API
#### Sobre esta solução:
- Esta solução foi desenvolvida por **Werner Moecke** para o desafio Ília.
- A solução é composta de 2 projetos:
    - **ControlePontoAPI:** Este é o projeto principal, contendo as Controllers, objetos e demais itens desenvolvidos.
    - **ControlePontoAPI.UnitTests:** Contém todos os testes unitários para o projeto principal.

#### Detalhes de implementação:
- Esta solução foi desenvolvida com o *target framework* **_.NET 6.0_**.
- Também foi optado por incluir a geração da documentação *OpenAPI v3.0* através do componente **_Swagger_**.
    - Uma vez que o projeto for executado no Visual Studio em modo **_Debug_**, a documentação poderá ser visualizada no browser:
        - *https://localhost:7000/swagger*, para conexão segura
        - *http://localhost:5000/swagger*, para conexão insegura
- Os 2 métodos expostos pela API poderão ser chamados pelo próprio *Swagger* (através de sua página aberta no navegador), ou também pelos seguintes endpoints:
    - *http[s]://localhost:[5000|7000]/v1/batidas*, passando no corpo da requisição, um objeto válido do tipo **_Momento_** (vide detalhes na documentação);
    - *http[s]://localhost:[5000|7000]/v1/folhas-de-ponto*, passando na *query* o valor do mês e ano no formato **_"yyyy-MM"_** (vide detalhes na documentação).