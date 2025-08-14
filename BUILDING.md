# Building Aegis Survivors

This document provides instructions for building the project for Android and iOS platforms.

## Prerequisites

*   **Unity Editor:** You must use **Unity 6000.1.14f1**.
*   **Platform Modules:** Ensure you have the "Android Build Support" and "iOS Build Support" modules installed for your Unity version via Unity Hub.
*   **Android SDK/NDK:** For Android builds, it's highly recommended to use the versions managed by Unity. Go to `Edit > Preferences > External Tools` and ensure the "Android SDK and NDK installed with Unity" checkboxes are enabled.
*   **Xcode:** For iOS builds, you must be on a macOS machine with a recent version of Xcode installed.

---

## Building for Android (.AAB)

The project is configured to produce an Android App Bundle (.AAB), which is required by the Google Play Store.

1.  **Open the Project:** Open the project in Unity 6000.1.14f1.
2.  **Switch Build Platform:** Go to `File > Build Settings`. Select "Android" from the list and click "Switch Platform".
3.  **Configure Player Settings:**
    *   Click the "Player Settings..." button.
    *   In the `Player > Other Settings` section:
        *   **Identification:** Set your `Package Name` (e.g., `com.yourcompany.aegissurvivors`).
        *   **Target API Level:** Set this to at least API Level 35.
    *   In the `Player > Publishing Settings` section:
        *   You must sign the application. Check the "Custom Keystore" box.
        *   Click "Keystore Manager..." and either create a new Keystore or select an existing one.
        *   **IMPORTANT:** Back up your Keystore file and passwords securely. If you lose them, you cannot update your app.
        *   Enter your Keystore password, and select your Key Alias and enter its password.
4.  **Build the Project:**
    *   Return to the `File > Build Settings` window.
    *   Ensure the scenes `MainMenuScene` and `GameScene` are in the "Scenes In Build" list, in that order.
    *   Click "Build". Unity will prompt you for a location to save the `.AAB` file.

---

## Building for iOS

Building for iOS is a two-step process: generating an Xcode project from Unity, and then building the final app from Xcode.

### Step 1: Generate Xcode Project from Unity

1.  **Open the Project:** Open the project in Unity 6000.1.14f1 on a macOS machine.
2.  **Switch Build Platform:** Go to `File > Build Settings`. Select "iOS" from the list and click "Switch Platform".
3.  **Configure Player Settings:**
    *   Click the "Player Settings..." button.
    *   In `Player > Other Settings`:
        *   **Identification:** Set your `Bundle Identifier` (e.g., `com.yourcompany.aegissurvivors`).
        *   **Signing:** Set your `Automatically Sign` option and provide your Apple Developer Team ID.
4.  **Build the Project:**
    *   Return to the `File > Build Settings` window.
    *   Click "Build". Unity will prompt you for a location to save the generated Xcode project folder.

### Step 2: Build from Xcode

1.  **Open the Xcode Project:** Navigate to the folder you saved in the previous step and open the `Unity-iPhone.xcodeproj` file.
2.  **Configure Signing & Capabilities:**
    *   In the "Signing & Capabilities" tab, ensure your Team and Bundle Identifier are correct. Xcode will use these to provision the app.
    *   Add any required capabilities (e.g., Game Center, iCloud).
3.  **Build and Run:**
    *   Select your target device (e.g., a connected iPhone or a simulator).
    *   Click the "Build and then run" button (the Play icon) in Xcode. Xcode will compile the project and install it on your target device.
