# fiap_grupo57_fase1

1 - Estrutura do projeto

```

fiap_grupo57_fase1/
├── Controllers
│   └── ContatosController.cs
├── Infrastructure
│   ├── Data
│   │   └── ContatoContext.cs
│   ├── Exception
│   │   ├── CustomException.cs
│   │   └── ExceptionMiddleware.cs
├── Interfaces
│   ├── Repositories
│   │   └── IContatosRepository.cs
│   └── Services
│       └── IContatosService.cs
├── Models
│   ├── Enum
│   │   └── RegiaoEnum.cs
│   ├── Requests
│   │   ├── ContatosPostRequest.cs
│   │   └── ContatosPutRequest.cs
│   ├── Responses
│   │   ├── ContatosGetResponse.cs
│   │   ├── ContatosPostResponse.cs
│   │   └── ExceptionResponse.cs
│   └── Entities
│       └── Contato.cs
├── Repositories
│   └── ContatosRepository.cs
├── Services
│   └── ContatosService.cs


fiap_grupo57_fase1_test/
├── UnitTests
│   ├── Services
│   │   └── ContatosServiceTests.cs
│   └── Repositories
│       └── ContatosRepositoryTests.cs
└── IntegrationTests
    └── Controllers
        └── ContatosControllerTests.cs
```

2 - Script para criar tabela no mysql:

```

CREATE TABLE dbmatheus.`contatos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Telefone` varchar(20) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `DDD` int NOT NULL,
  `Regiao` int NOT NULL,
  PRIMARY KEY (`Id`),
  FOREIGN KEY (`Regiao`) REFERENCES `regioes`(`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE dbmatheus.`regioes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

```