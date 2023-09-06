# WpfTestTaskAdvantica

## Структура
- LibProto — библиотека с контрактом gRPC.
- GrpcServerTestTaskAdvantica — сервер.
- WpfTestTaskAdvantica — клиент.

## Как запустить
1. Развернуть базу данных в проекте GrpcServerTestTaskAdvantica. Для этого ввести в консоли 
- cd GrpcServiceTestTaskAdvantica
- dotnet ef database update
2. Запустить GrpcServerTestTaskAdvantica без отладки через контекстное меню проекта.
3. Запустить WpfTestTaskAdvantica любым способом. 
