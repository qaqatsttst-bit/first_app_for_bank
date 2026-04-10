# 05_role_permission_matrix.md

## 1. Назначение документа

Документ фиксирует детальную permission-matrix V1.

---

## 2. Роли

- `Operator`
- `TechnicalSpecialist`
- `ServiceOwner`
- `Administrator`

---

## 3. Permission matrix

### 3.1. View permissions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| View dashboard | Yes | Yes | Yes | Yes |
| View catalog | Yes | Yes | Yes | Yes |
| View service card | Yes | Yes | Yes | Yes |
| View status history | Yes | Yes | Yes | Yes |
| View timeline | Yes | Yes | Yes | Yes |
| View comments | Yes | Yes | Yes | Yes |
| View service links | Yes | Yes | Yes | Yes |
| View metrics | Yes | Yes | Yes | Yes |
| View role-safe diagnostics | Yes | Yes | Yes | Yes |
| View admin diagnostics | No | No | No | Yes |

### 3.2. Comment / incident-context actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Add comment | No | Yes | Yes (own service) | Yes |
| Confirm problem | No | Yes | No | Yes |
| Revoke confirmation | No | Yes | No | Yes |

### 3.3. Link actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Edit `runbook` | No | No | Yes (own service) | Yes |
| Edit `docs` | No | No | Yes (own service) | Yes |
| Edit `dashboard` | No | No | Yes (own service) | Yes |
| Edit `logs` | No | No | No | Yes |
| Edit `alerts` | No | No | No | Yes |
| Edit `other` | No | No | No | Yes |

### 3.4. Service governance actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Create service | No | No | No | Yes |
| Activate/deactivate service | No | No | No | Yes |
| Change owner | No | No | No | Yes |
| Change category | No | No | No | Yes |
| Change criticality | No | No | No | Yes |
| Change service_type | No | No | No | Yes |
| Change integration keys | No | No | No | Yes |

### 3.5. User/role actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Assign role | No | No | No | Yes |
| Remove role | No | No | No | Yes |
| Deactivate user | No | No | No | Yes |

### 3.6. Maintenance / admin actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Start maintenance | No | No | No | Yes |
| End maintenance | No | No | No | Yes |
| Redact comment | No | No | No | Yes |
| Run backend-only admin flow | No | No | No | Yes / trusted ops only |

---

## 4. Enforcement rule

Все permissions должны применяться:
- в UI;
- в backend authorization;
- в application use cases.

UI-only hiding не считается enforcement.

---

## 5. Consistency rule

Этот документ обязан быть синхронизирован с `01_project_foundation.md`.
