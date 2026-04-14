# PJATK-APBD-Cw5-s32797

Projekt wykonany w ramach zajęć **APBD**.  
Aplikacja została przygotowana jako **ASP.NET Core Web API** oparta na kontrolerach.

## Opis
API służy do zarządzania:
- salami (`Rooms`)
- rezerwacjami (`Reservations`)

Dane są przechowywane **w pamięci aplikacji** — bez użycia bazy danych i Entity Framework Core.

## Funkcjonalności

### Rooms
- pobieranie wszystkich sal
- pobieranie sali po `id`
- pobieranie sal po kodzie budynku
- filtrowanie sal po parametrach query string
- dodawanie nowej sali
- aktualizacja sali
- usuwanie sali

### Reservations
- pobieranie wszystkich rezerwacji
- pobieranie rezerwacji po `id`
- filtrowanie rezerwacji po dacie, statusie i `roomId`
- dodawanie nowej rezerwacji
- aktualizacja rezerwacji
- usuwanie rezerwacji

## Reguły biznesowe
- nie można dodać rezerwacji dla sali, która nie istnieje
- nie można dodać rezerwacji dla sali nieaktywnej
- rezerwacje tej samej sali nie mogą nakładać się czasowo tego samego dnia
- nie można usunąć sali, jeśli ma powiązane rezerwacje
