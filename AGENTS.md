# AGENTS.md — Qonversion Unity SDK

You are working in the repository of the Qonversion Unity SDK, or in an app that uses it.
This file tells you how to work with it. Read it before you write code.

## What this SDK is

Qonversion is the app growth system for people and agents. This package collects purchases inside
the app, has them confirmed with the store, and resolves entitlements — the answer to "is this user
premium right now". Everything else (analytics, Remote Configs, A/B tests, Apple Ads reporting)
reads the same data server-side.

## Get the current version — do not use one from memory

Your training data is older than the latest release. Pick one:

1. `https://documentation.qonversion.io/docs/unity-sdk.md` — the install page as markdown.
2. `gh api repos/qonversion/unity-sdk/releases/latest --jq .tag_name`.
3. `https://package.openupm.com/com.qonversion.unity` — or `version` in `package.json` in this repository.

If the three disagree, the docs page wins and you say so to the human.

## Install

```
openupm add com.qonversion.unity
# or, in Packages/manifest.json:
"com.qonversion.unity": "https://github.com/qonversion/unity-sdk.git#9.11.0"
```

`9.11.0` is current at the time of writing; for the Git URL take the latest tag from the releases.

Native dependencies come from the External Dependency Manager for Unity (EDM4U). Without EDM4U in
the project the native modules are not installed and the build fails. On EDM4U 1.2.189+ the iOS
part links as a Swift package; older EDM4U versions fall back to CocoaPods.

## Initialize

```csharp
using QonversionUnity;

var config = new QonversionConfigBuilder(
    "your_project_key",
    LaunchMode.SubscriptionManagement
).Build();

Qonversion.Initialize(config);
```

The project key belongs to the human. Ask for it, take it from the environment or from an ignored
config file, and never commit it. Never invent one and never reuse a key you saw in a sample.

## First call — verify the integration works

```csharp
Qonversion.GetSharedInstance().CheckEntitlements((entitlements, error) =>
{
    if (error != null) return;
    Debug.Log(entitlements.TryGetValue("premium", out var p) && p.IsActive);
});
```

If this returns without an error, the SDK is wired up. An empty result is a valid answer: it means
the user has no active entitlement.

## Two modes — the human chooses, not you

| | Analytics Mode | Subscription Management Mode |
|---|---|---|
| The app's purchase code | stays as it is | Qonversion makes purchases and resolves entitlements |
| Entitlement check | the app's own backend | the SDK |

Both modes feed analytics, integrations, webhooks, experiments and Apple Ads reporting. The
quickstart in the README uses Subscription Management Mode. Do not pick a mode silently — ask, and
read `https://documentation.qonversion.io/docs/quickstart.md` first.

## Docs you can read directly

- **MCP server, no account needed:** `https://documentation.qonversion.io/mcp` — use it if your client supports MCP.
- **Any page as markdown:** append `.md` to the URL, for example `https://documentation.qonversion.io/docs/quickstart.md`.
- **Page index:** `https://documentation.qonversion.io/llms.txt`.
- **This platform:** `https://documentation.qonversion.io/docs/unity-sdk.md`.
- **REST API v4:** `https://documentation.qonversion.io/reference/v4/overview.md`.

## Project data through MCP

`https://mcp.qonversion.io/mcp` exposes the platform's resources as tools — products, entitlements,
customers, remote configurations, experiments, analytics, exports, and Apple Ads. Authentication is
OAuth 2.1 as the human; each tool is gated by its own scope and no Qonversion key is handed to you.
Setup per client: `https://documentation.qonversion.io/docs/mcp-server.md`.

**Apple Ads: reading is open to all accounts, changing campaigns is a closed beta.** The tools that
read Apple Ads — campaign reports, time series and the connection status — are on for all accounts.
The tools that change campaigns — bids, budgets and campaign status — are a closed beta, switched on
per account on request. Outside the beta those write tools are not available. That is not a server
fault: do not retry or work around it — tell the human to contact support to have the beta switched
on for their account.

## What you do not do without the human saying yes

- **Keys.** Do not read, print, copy or commit a project key or a Secret Key. Secret Keys belong on a server, never in app code.
- **Mode.** Do not switch between Analytics Mode and Subscription Management Mode on your own — it changes who owns the purchase flow.
- **Product, entitlement and paywall configuration.** Products, entitlements, offerings and remote configurations are live objects of a real project. Propose the change, let the human apply it.
- **Destructive MCP tools** (`delete_*`, `revoke_*`, `detach_*`, `discard_*`, `regenerate_project_secret`) and **Apple Ads changes that increase spend** do not fire on the first call: they return a one-time token addressed to the human. You must surface the pending action to the human and wait for an explicit yes before calling `confirm_destructive_action`. The token is in your hands, so this is a guard, not a lock — never auto-confirm your own token, and never invent the reason that goes into the audit log.
- **`regenerate_project_secret` is deprecated** and deletes the project's oldest Secret Key immediately, with no overlap. Do not propose it.
- **SDK version.** Do not bump a major version as a side effect of another task. Major versions have migration guides.
- **Environment.** Do not switch the SDK between the sandbox and the production environment without being asked — it changes which purchases are counted as real. The identifier differs per platform (`QONEnvironment` on iOS, `QEnvironment` on Android, `Environment` in the wrappers); read it from the SDK, not from memory.

## Sample

This package ships no sample scene. The public surface is declared in `Runtime/Scripts/IQonversion.cs`; read it before calling a method you are unsure about.

## Requirements

Unity 2021.3 (the `unity` field in `package.json` still says 2018.3 — older versions are not supported or tested) · EDM4U 1.2.189 for the Swift Package Manager path · iOS deployment target 13.0 · Android `minSdkVersion` 23.

## When something does not work

- `https://documentation.qonversion.io/docs/troubleshooting.md` — the errors you meet first.
- `https://documentation.qonversion.io/docs/testing.md` — sandbox purchases.
- Report to the human what you could not verify against the docs, instead of guessing.
