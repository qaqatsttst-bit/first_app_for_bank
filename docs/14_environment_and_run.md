# 14_environment_and_run.md

## 1. Назначение документа

Документ фиксирует рамку и обязательные требования для темы локального запуска, environment configuration, database bootstrap, migrations, bootstrap первого администратора, минимальной проверки после старта и стартового troubleshooting для V1.

Этот документ является governing / overview document для темы environment and run.

## 2. Что должен покрывать документ

Документ должен покрывать:
- минимальные требования к окружению;
- обязательные configuration groups;
- database bootstrap expectations;
- migrations expectations;
- bootstrap первого администратора;
- seed/demo/test assumptions;
- базовую post-start verification рамку;
- troubleshooting categories;
- требования к обязательным связанным run-артефактам.

## 3. Минимальные требования к окружению

Для V1 предполагаются:
- .NET SDK совместимой версии с проектом;
- PostgreSQL, доступный приложению;
- доступ к OIDC provider или допустимому dev substitute для среды разработки;
- доступ к status source или controlled mock/stub для dev/test среды;
- доступ к Prometheus или controlled degraded mode, если реальный metrics backend недоступен.

## 4. Configuration groups

Должна существовать конфигурация для:
- PostgreSQL connection string;
- OIDC issuer/client settings;
- status source base URL / auth / timeout / polling;
- Prometheus base URL / query timeout;
- bootstrap admin settings;
- secret storage location.

## 5. Правила работы с конфигурацией

- Секреты не должны жить в коде.
- Секреты не должны коммититься в репозиторий в открытом виде.
- Конфигурация окружения должна быть разделена минимум на local/dev, integration/test и production-like.
- Отсутствие обязательной конфигурации должно приводить к предсказуемому fail-fast там, где это нужно для безопасного старта.

## 6. Database bootstrap

Для первого запуска среды должна существовать возможность:
1. создать БД;
2. применить migrations;
3. проверить доступность схемы;
4. убедиться, что обязательные таблицы V1 присутствуют;
5. убедиться, что базовые reference data и/или справочники могут быть подготовлены.

## 7. Bootstrap первого администратора

Для V1 должен существовать bootstrap-механизм первого администратора.

Минимальные правила:
- trusted bootstrap user определяется конфигурацией;
- при первом успешном входе такой пользователь может получить роль администратора;
- факт назначения должен фиксироваться в audit и technical logs;
- после появления первого active admin bootstrap-механизм должен быть отключён или исчерпан.

## 8. Run modes

### 8.1. Local developer mode
Допускаются локальная БД, mock/stub интеграций и controlled degraded mode.

### 8.2. Integration/test mode
Желательно использовать real schema migrations и близкую конфигурацию auth/integration.

### 8.3. Production-like mode
Секреты берутся только из безопасного storage/configuration, а bootstrap/admin paths явно контролируются.

## 9. Minimum developer startup flow

1. подготовить локальное окружение и конфигурацию;
2. поднять PostgreSQL;
3. применить canonical migrations path;
4. подготовить bootstrap/reference data, если это требуется;
5. запустить приложение canonical startup path;
6. выполнить login через OIDC или approved dev substitute;
7. пройти post-start verification checklist;
8. убедиться, что degraded behavior корректно срабатывает для отсутствующих интеграций.

## 10. Минимальная проверка после запуска

После запуска приложения должно быть возможно проверить:
- приложение стартует без критической ошибки;
- PostgreSQL доступен;
- migrations применены корректно;
- OIDC configuration валидна;
- bootstrap admin работает в пределах разрешённой модели;
- status source либо доступен, либо система корректно входит в ожидаемый degraded mode;
- Prometheus либо доступен, либо metrics section может безопасно деградировать.

## 11. Обязательные связанные артефакты

Для V1 репозиторий должен содержать:
- `LOCAL_SETUP.md` или README-раздел с literal local startup steps;
- configuration reference;
- migration / bootstrap artifact;
- troubleshooting artifact.

## 12. Troubleshooting categories

### 12.1. Database unavailable
- проверить connection string;
- проверить доступность PostgreSQL;
- проверить, что migrations были применены.

### 12.2. OIDC misconfiguration
- проверить issuer/client settings;
- проверить обязательные claims;
- проверить redirect/callback settings.

### 12.3. Bootstrap admin not assigned
- проверить bootstrap configuration;
- проверить trusted bootstrap list;
- проверить, что первый active admin ещё не существует.

### 12.4. Status source unavailable
- проверить endpoint/auth configuration;
- проверить timeout/retry configuration;
- проверить, что система переходит в ожидаемый degraded/stale behavior.

### 12.5. Prometheus unavailable
- проверить endpoint/query timeout;
- убедиться, что metrics section деградирует безопасно.
