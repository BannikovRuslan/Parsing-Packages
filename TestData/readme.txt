Пока я не добавил задачу на доску, скидываю примеры файлов, которые нужно будет распарсить:
1. Для npm-пакетов:
https://github.com/Flexberry/ember-flexberry/blob/feature-ember-update/package.json

Парсить нужно секции dependencies, devDependencies и peerDependencies (при наличии любой из них).

Подробнее про структуру файла pacakge.json можно почитать тут (https://docs.npmjs.com/cli/v6/configuring-npm/package-json?v=true). 

2. Для NuGet-пакетов:
a. csproj-файлы:
https://github.com/Flexberry/Flexberry.SampleForMetrics/blob/main/src/SampleForMetrics/Objects/SampleForMetrics.Objects.csproj
https://github.com/Flexberry/NewPlatform.Flexberry.Caching/blob/develop/NewPlatform.Flexberry.Caching/NewPlatform.Flexberry.Caching.csproj
Информация о NuGet-пакетах может хранится в тегах Reference или PackageReference.

Подробнее про Reference можно почитать тут (https://learn.microsoft.com/ru-ru/archive/blogs/manishagarwal/resolving-file-references-in-team-build-part-2).
Подробнее про PackageReference можно почитать тут (https://learn.microsoft.com/ru-ru/nuget/consume-packages/package-references-in-project-files).


b. packages.config
https://github.com/Flexberry/Flexberry.ServiceBus.Sample/blob/master/RESTAdapter/MsgSender/MsgSender/packages.config
Побробнее можно почитать тут (https://learn.microsoft.com/ru-ru/nuget/reference/packages-config).

Информацию по NuGet-пакетам в проекте мы пытаемся достать одновременно и из csproj-файлов, и из package.config-файлов (если есть).

3. Для Dockerfile:
https://github.com/Flexberry/Flexberry.SampleForSuperset/blob/main/src/Docker/Dockerfile
https://github.com/Flexberry/Flexberry.SampleForSuperset/blob/main/src/Docker/Dockerfile.Superset

Имена файлов могут иметь формат Dockerfile или Dockerfile.*

Парсить нужно инструкции FROM.

Подробнее можно почитать тут (https://docs.docker.com/engine/reference/builder/).

P.S. Предлагаю алгоритм поиска файлов вынести в отдельный метод, который можно будет потом подменить, чтобы потом искать файлы не в конкретной папке на локальной машине, а в удаленных репозиториях (это будет сделующий шаг реализации).