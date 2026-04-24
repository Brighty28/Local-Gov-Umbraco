# LocalGov Umbraco

> The Umbraco CMS equivalent of [LocalGov Drupal](https://localgovdrupal.org/) — an open-source distribution for UK local government websites.

[![CI](https://github.com/Brighty28/Local-Gov-Umbraco/actions/workflows/ci.yml/badge.svg)](https://github.com/Brighty28/Local-Gov-Umbraco/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/LocalGov.Umbraco)](https://www.nuget.org/packages/LocalGov.Umbraco/)

## What is this?

LocalGov Umbraco provides a complete, production-ready set of Umbraco 13 packages for UK councils, built on the **GOV.UK Frontend design system** and targeting **WCAG 2.1 AA** compliance.

It is designed as a direct alternative to LocalGov Drupal for councils already running Umbraco — and is particularly relevant for councils going through **Local Government Reorganisation (LGR)**.

## Quick Start

```bash
dotnet add package LocalGov.Umbraco
```

This installs all Phase 1 modules. Umbraco's `IComposer` auto-discovery wires everything up — no `Program.cs` changes needed. On first run, uSync materialises the content type definitions automatically.

## Packages

| Package | Description |
|---|---|
| `LocalGov.Umbraco.Core` | Shared compositions, `ILocalGovContentHelper`, navigation, search |
| `LocalGov.Umbraco.Theme` | GOV.UK Frontend v5, per-council branding via CSS variables |
| `LocalGov.Umbraco.Services` | Service landing pages, sub-landing pages, content pages |
| `LocalGov.Umbraco.News` | News articles, newsroom archive, category filtering |
| `LocalGov.Umbraco.AlertBanner` | 4 severity types, dismissible, scheduled alerts |
| `LocalGov.Umbraco.Guides` | Multi-page guides with sidebar navigation |
| `LocalGov.Umbraco.StepByStep` | Numbered step-by-step processes |
| `LocalGov.Umbraco.ContentReview` | Content review governance with email reminders |

### Coming in Phase 2

`LocalGov.Umbraco.Events` · `LocalGov.Umbraco.Directories` · `LocalGov.Umbraco.Elections` · `LocalGov.Umbraco.GeoLocation` · `LocalGov.Umbraco.Forms` · `LocalGov.Umbraco.MultiAuthority`

## Why LocalGov Umbraco?

- **LGR-ready** — `MultiAuthority` module (Phase 2) supports multi-root content trees for predecessor council sites alongside the new combined authority
- **Content governance** — `ContentReview` package provides the audit trail needed during LGR migration, including `migrationSource` field and automated email reminders
- **GOV.UK Frontend** — the shared design vocabulary of all UK government digital teams; any council's IT staff can immediately contribute
- **Proven patterns** — core architecture extracted from a production council site (South Cambridgeshire District Council) running Umbraco 13

## Requirements

- Umbraco 13.x (net8.0)
- uSync 13.x
- SQL Server or SQLite

## Contributing

This project is open for contributions from councils, Umbraco developers, and the wider local government digital community. Please open an issue before submitting a pull request.

## Licence

MIT — free to use, modify, and distribute.
