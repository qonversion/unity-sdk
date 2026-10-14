<p align="center"><img src=".github/assets/sdk-header.png" width="100%" alt="Qonversion SDK"></p>

# Qonversion Unity SDK

**Qonversion is the app growth system for people and agents.** It collects the revenue data of your
subscription app and keeps it lined up with what the App Store and Google Play report, so campaigns,
experiments and entitlements run on the same numbers. This repository holds the Unity SDK: the
part that runs inside your app.

[![Version](https://img.shields.io/github/v/release/qonversion/unity-sdk?label=version)](https://github.com/qonversion/unity-sdk/releases)
[![openupm](https://img.shields.io/npm/v/com.qonversion.unity?registry_uri=https://package.openupm.com&label=openupm)](https://openupm.com/packages/com.qonversion.unity/)
[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](LICENSE)
[![Platforms](https://img.shields.io/badge/platforms-iOS%20%7C%20Android-blue.svg?style=flat)](https://documentation.qonversion.io/docs/unity-sdk)

## Quickstart

Create a project in the [dashboard](https://dash.qonversion.io) and connect your stores first — nothing validates until then.

**1. Install**

```bash
openupm add com.qonversion.unity
```

Or by Git URL in `Packages/manifest.json`: `"com.qonversion.unity": "https://github.com/qonversion/unity-sdk.git#9.11.0"` — current at the time of writing; take the latest tag from [Releases](https://github.com/qonversion/unity-sdk/releases).
Native dependencies are resolved by the External Dependency Manager for Unity (EDM4U) — it must be
in the project, otherwise the native modules are not installed and the build fails.

**2. Initialize**

```csharp
using QonversionUnity;

var config = new QonversionConfigBuilder(
    "your_project_key",
    LaunchMode.SubscriptionManagement
).Build();

Qonversion.Initialize(config);
```

The project key is in the dashboard under **Settings → Developer → Keys**.

This is Subscription Management Mode — Qonversion owns the purchase flow. To keep your own purchase
code and use Qonversion for analytics only, initialize with [Analytics Mode](https://documentation.qonversion.io/docs/analytics-mode) instead.

**3. Check what the user has access to**

```csharp
Qonversion.GetSharedInstance().CheckEntitlements((entitlements, error) =>
{
    if (error != null) return;

    if (entitlements.TryGetValue("premium", out var premium) && premium.IsActive)
    {
        // unlock premium features
    }
});
```

Full walk-through, both modes and the store setup: [Quick start](https://documentation.qonversion.io/docs/quickstart).

### iOS native dependencies

With EDM4U 1.2.189 and newer the iOS part is linked as a Swift package; older EDM4U versions fall
back to CocoaPods — see
[Install Native Dependencies](https://documentation.qonversion.io/docs/unity-sdk#install-native-dependencies).

> CocoaPods trunk becomes read-only on December 2, 2026 — see the
> [CocoaPods → Swift Package Manager migration guide](https://documentation.qonversion.io/docs/dec-2026-migration-guide-cocoapods-to-spm).

## Documentation

* [Welcome](https://documentation.qonversion.io/docs/qonversion-platform) — what the platform does and the three ways to start.
* [Unity setup](https://documentation.qonversion.io/docs/unity-sdk) — full installation for this platform.
* [Quick start](https://documentation.qonversion.io/docs/quickstart) — choose a mode, create a project, connect the stores.
* [API reference](https://documentation.qonversion.io/reference/v4/overview) — REST API v4.
* [For agents](https://documentation.qonversion.io/docs/for-agents) — MCP servers, machine-readable docs, ready-made prompts.

## What this SDK does

* **Collects purchases** and sends them to Qonversion, where they are confirmed with the store instead of being trusted from the device.
* **Resolves entitlements** — one answer to "is this user premium right now", the same on every device the user signs in to.
* **Keeps the history** — trials, conversions, renewals, cancellations, refunds, upgrades and downgrades, as events you can query, export or forward.
* **Feeds Remote Configs and A/B tests** — change which products, paywalls and app behaviour your users see, and run A/B tests without shipping a release.
* **Feeds Apple Ads reporting** — see what each campaign, ad group and keyword returns in real revenue, not only in installs.
* **Renders screens built in the dashboard** — paywalls and onboarding, updated without shipping a release.

What happens when something fails — the standby service, fallback files and the uptime commitment
behind the platform: [reliability](https://qonversion.io/reliability).

## For agents

An agent can integrate this SDK end to end. The companion file [`AGENTS.md`](AGENTS.md) gives it the
current version, the documented method names and the rules it must not break, instead of what its
training data remembers.

**Read these docs as an agent.** The documentation exposes an MCP server at
`https://documentation.qonversion.io/mcp` — no account needed. Any page is available as markdown by
appending `.md` to its URL, and [`llms.txt`](https://documentation.qonversion.io/llms.txt) lists every page.

**Work on your project data as an agent.** The Qonversion MCP server at `https://mcp.qonversion.io/mcp`
exposes the platform's resources as tools. The agent authenticates as you over OAuth 2.1, each tool
is gated by its own scope, and no Qonversion key is handed to the model. Apple Ads tools need the
feature on your account (404 otherwise); campaign changes additionally need the `apple_ads:write`
scope and a team member with campaign-management permission — check
`get_apple_ads_connection_status`.

Destructive tools and Apple Ads changes that increase spend do not fire on the first call: they
return a short-lived token addressed to the human operator, and completing the action requires
echoing the resource id verbatim and giving a reason, which is recorded in the audit log. Pausing an
entity or lowering a value is **not** parked — it applies immediately. The full tool list, the
per-client setup and the scopes are in
[MCP Server](https://documentation.qonversion.io/docs/mcp-server).

**Have an agent integrate this SDK.** Paste this into Claude Code, Cursor, Codex or any coding agent
working in your app's repository:

> Integrate the Qonversion SDK into this app.
>
> Read the official documentation before writing any code. The docs expose an MCP server at `https://documentation.qonversion.io/mcp` — use it if you can. Otherwise fetch pages directly; any page is available as markdown by appending `.md` to its URL, and `https://documentation.qonversion.io/llms.txt` lists every page.
>
> Steps:
> 1. Check with me that a Qonversion project exists and the app stores are connected — project key, App Store shared secret and App Store ID, Google Play service account key. This is step 2 of the quick start, I have to do it in the dashboard, and nothing validates until it is done. Google credentials can take up to 24 hours to start working.
> 2. Read `https://documentation.qonversion.io/docs/quickstart.md` and decide with me between Analytics Mode and Subscription Management Mode. Do not pick one silently.
> 3. Read the install page for this project's platform, linked from `https://documentation.qonversion.io/docs/install-sdk.md`, and follow its steps and method names. Take the SDK version from the latest release tag or the package registry, not from memory — the docs page can name an older one; if they disagree, tell me.
> 4. Install and configure the SDK. Ask me for the Qonversion project key; it is in the dashboard under Settings → Developer → Keys. Never commit it to the repository.
> 5. Implement the mode we chose, following the documented method names exactly.
> 6. Show me the diff and tell me what you could not verify against the docs.

Prefer to do it by hand? The steps are in the [quick start](https://documentation.qonversion.io/docs/quickstart).

## Sample app

A Unity project that initializes the SDK, lists products, makes a purchase and reads entitlements
lives in a separate repository: [qonversion/unity-sdk-sample](https://github.com/qonversion/unity-sdk-sample)
(`Assets/Scripts/SampleApp.cs`). It loads the SDK from a sibling checkout (`file:../../unity-sdk`), so
clone this repository next to it. It runs in the sandbox environment; for your own initialization, use
the Quickstart snippet above.

## Requirements

| | Minimum |
|---|---|
| Unity | 2021.3 |
| EDM4U for the Swift Package Manager path | 1.2.189 |
| iOS deployment target | 13.0 |
| Android `minSdkVersion` | 23 (inherited from the Android SDK) |

The `unity` field in `package.json` still says 2018.3; Unity versions below 2021.3 are not supported
or tested.

## Help

Contact us via [issues on GitHub](https://github.com/qonversion/unity-sdk/issues) or the
[contact form](https://qonversion.io/contact-us) on our site.

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/SuperFeature`)
3. Commit your Changes. Use small commits with separate logic. (`git commit -m 'Add some super feature'`)
4. Push to the Branch (`git push origin feature/SuperFeature`)
5. Open a Pull Request

## License

Qonversion SDK is available under the MIT license. See [LICENSE](LICENSE).
