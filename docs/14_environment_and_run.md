# 14_environment_and_run.md

## 1. Назначение документа

Документ фиксирует правила локального запуска, environment configuration, database bootstrap, migrations, seed/bootstrap assumptions и минимальные verification steps для V1.

Этот документ является основным источником истины для:
- local developer run;
- environment prerequisites;
- configuration boundaries;
- migrations/bootstrap flow;
- post-start verification.

Он не заменяет:
- operational admin runbook (`12_run_guide.md`);
- NFR/policy thresholds (`10_non_functional_requirements.md`);
- error/degraded taxonomy (`11_error_handling_and_degraded_mode.md`).

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

---

## 2. Scope документа

### 2.1. Что покрывается

Документ покрывает:
- минимальные требования к локальному окружению;
- обязательные configuration groups;
- database bootstrap;
- migrations;
- bootstrap первого администратора;
- seed/demo/test assumptions;
- базовую проверку после запуска;
- troubleshooting стартовых проблем.

### 2.2. Что не покрывается

Документ не покрывает:
- product boundaries и business rules;
- детальную permission matrix;
- детальную taxonomy degraded/error states;
- operational execution backend-only admin actions в production-like контуре.

---

## 3. Минимальные требования к окружению

Для V1 предполагаются:
- .NET SDK совместимой версии с проектом;
- PostgreSQL, доступный приложению;
- доступ к OIDC provider или безопасный dev substitute для среды разработки;
- доступ к status source или controlled mock/stub для dev/test среды;
- доступ к Prometheus или controlled degraded/test mode, если real metrics backend недоступен.

Точные версии SDK/runtime и infra-specific details должны фиксироваться рядом с реализацией и CI/CD конфигурацией.

---

## 4. Configuration groups

### 4.1. Database configuration

Должна существовать конфигурация для:
- PostgreSQL connection string;
- database name/schema settings, если используются;
- migration execution context.

### 4.2. OIDC configuration

Должна существовать конфигурация для:
- issuer;
- client identifier;
- client secret или иного безопасного server-side способа конфигурации;
- callback/redirect settings;
- required claims expectations.

### 4.3. Status source configuration

Должна существовать конфигурация для:
- status source base URL;
- authentication settings;
- request timeout;
- polling interval;
- retry settings в допустимых границах.

### 4.4. Metrics configuration

Должна существовать конфигурация для:
- Prometheus base URL;
- query timeout;
- default range/step behavior, если это требуется приложением;
- safe degraded handling при недоступности источника.

### 4.5. Security / bootstrap configuration

Должна существовать конфигурация для:
- `BOOTSTRAP_ADMIN_EMAILS` и/или trusted `sub`;
- session / re-auth settings в пределах политики окружения;
- storage location секретов вне исходного кода.

---

## 5. Правила работы с конфигурацией

- Секреты не должны жить в коде.
- Секреты не должны коммититься в репозиторий в открытом виде.
- Конфигурация окружения должна быть разделена минимум на:
  - local/dev
  - integration/test
  - production-like
- Значения `issuer`, secrets, endpoints и trusted identifiers относятся к environment-specific деталям и не должны хардкодиться в foundation docs.

---

## 6. Database bootstrap

Для первого запуска среды должна существовать возможность:
1. создать БД;
2. применить migrations;
3. проверить доступность схемы;
4. убедиться, что обязательные таблицы V1 присутствуют;
5. убедиться, что базовые reference data и/или справочники могут быть подготовлены.

Если migrations не применились успешно, среда не считается готовой к запуску прикладных сценариев.

---

## 7. Migrations policy

- Изменения schema-level модели должны доставляться через управляемые migrations.
- Migration flow должен быть repeatable и предсказуемым.
- Migration execution не должен скрыто выполнять неаудируемые административные business changes, если такие изменения относятся к backend-only administrative flows.
- `migration-assisted assignment` допустим как controlled path только в пределах правил foundation и admin run guide.

---

## 8. Bootstrap первого администратора

Для V1 должен существовать bootstrap-механизм первого администратора.

Минимальные правила:
- trusted bootstrap user определяется конфигурацией;
- при первом успешном входе такой пользователь получает роль администратора;
- факт назначения должен фиксироваться в audit;
- факт назначения должен фиксироваться и в technical logs;
- после появления первого active admin bootstrap-механизм должен быть отключён или исчерпан.

---

## 9. Seed / demo / test assumptions

### 9.1. Controlled seed

`Controlled seed` допустим для:
- reference data;
- initial categories;
- предопределённого справочника ролей;
- test/dev data, если это контролируемо и не противоречит foundation.

### 9.2. Demo/test data boundary

Demo/test data:
- не должны маскироваться под production truth;
- не должны ломать activation rules;
- не должны подменять real audit expectations для production-like среды.

### 9.3. Seed and audit

Если seed выполняет административно значимое business change в production-like или shared environment, должны соблюдаться правила auditability и controlled execution.

---

## 10. Run modes

### 10.1. Local developer mode

В local developer mode допускается:
- использование локальной БД;
- использование mock/stub внешних интеграций;
- controlled degraded mode для отсутствующих интеграций;
- запуск без полного production perimeter.

### 10.2. Integration/test mode

В integration/test mode желательно:
- использовать real schema migrations;
- использовать максимально близкую конфигурацию auth/integration;
- проверять поведение degraded mode на реальных integration boundaries.

### 10.3. Production-like mode

В production-like среде:
- секреты берутся только из безопасного storage/configuration;
- bootstrap/admin paths должны быть явно контролируемы;
- не допускается ad hoc изменение конфигурации вне управляемого процесса.

---

## 11. Минимальная проверка после запуска

После запуска приложения должно быть возможно проверить:
- приложение стартует без критической ошибки;
- PostgreSQL доступен;
- migrations применены корректно;
- OIDC configuration валидна;
- локальный пользователь может быть создан при первом успешном входе;
- bootstrap admin работает в пределах разрешённой модели;
- status source либо доступен, либо система корректно входит в ожидаемый degraded mode;
- Prometheus либо доступен, либо metrics section может безопасно деградировать.

---

## 12. Recommended startup verification checklist

Минимально рекомендуется проверить:
1. открывается главная страница;
2. каталог загружается;
3. карточка сервиса остаётся usable без metrics;
4. role enforcement работает серверно;
5. пользователь без роли не получает доступ;
6. первый bootstrap admin может быть создан корректно;
7. отсутствие status source приводит к ожидаемому stale/unknown поведению;
8. отсутствие Prometheus не ломает Overview / History / Comments / Timeline / Links.

---

## 13. Troubleshooting

### 13.1. Database unavailable

Если БД недоступна:
- проверить connection string;
- проверить доступность PostgreSQL;
- проверить, что migrations были применены;
- не считать среду готовой, пока schema verification не успешна.

### 13.2. OIDC misconfiguration

Если аутентификация не работает:
- проверить issuer/client settings;
- проверить обязательные claims;
- проверить redirect/callback settings;
- проверить, что локальная модель пользователя создаётся и связывается корректно.

### 13.3. Bootstrap admin not assigned

Если первый администратор не назначается:
- проверить bootstrap configuration;
- проверить, что пользователь действительно входит в trusted bootstrap list;
- проверить, что первый active admin ещё не существует;
- проверить audit/technical logs.

### 13.4. Status source unavailable

Если недоступен status source:
- проверить endpoint/auth configuration;
- проверить timeout/retry configuration;
- проверить, что система переходит в ожидаемый degraded/stale behavior, а не падает целиком.

### 13.5. Prometheus unavailable

Если недоступен Prometheus:
- проверить endpoint/query timeout;
- убедиться, что metrics section деградирует безопасно;
- убедиться, что `current_status` не ломается только из-за metrics failure.

---

## 14. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `10_non_functional_requirements.md`
- `11_error_handling_and_degraded_mode.md`
- `12_run_guide.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
