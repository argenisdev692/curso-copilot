# Fundamentos de la Filosofía de Prompts Avanzados para Testing

## 1. Test-First Thinking
Esta filosofía implica definir el comportamiento esperado del código antes de escribir la implementación. Los prompts deben enfocarse en especificar qué debe hacer el código, no cómo. Por ejemplo, en lugar de pedir "crea una función que sume dos números", se especifica "crea una función que tome dos enteros y retorne su suma, lanzando excepción si alguno es negativo".

## 2. Arrange-Act-Assert Pattern
Los tests deben estructurarse claramente en tres fases: preparación de datos y mocks (Arrange), ejecución del método bajo prueba (Act), y verificación de resultados (Assert). Los prompts avanzados incluyen esta estructura para generar tests legibles y mantenibles.

## 3. Mocking Strategy
Se debe aislar la unidad bajo test usando mocks para dependencias externas. Los prompts especifican qué interfaces mockear y cómo configurar los comportamientos esperados, evitando tests frágiles que dependan de implementaciones reales.

## 4. Coverage-Driven
Los prompts guían la generación de tests que aumenten métricas de cobertura, identificando casos edge y escenarios de error. Se enfoca en cubrir ramas condicionales, excepciones y validaciones.

## 5. BDD Approach
Los tests deben ser legibles como documentación de comportamiento. Los prompts usan lenguaje natural para describir escenarios Given-When-Then, haciendo que los tests sirvan como especificación viva del sistema.