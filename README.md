# 🌌 BIP - GRAVITY: Collaborative Guide & README

Welcome to **BIP - GRAVITY**! This is a Unity 2D (Unity 6: **6000.4.2f1**) physics-survival game.

Whether this is your first game, your first time using Unity, or your first time collaborating on GitHub, **don't panic!** This guide is designed to "baby-proof" our project so we can all build together without deleting each other's work.

---

## 🛑 THE THREE GOLDEN RULES (Read This or Explode)

### RULE 1: The "Scene Protocol" (Never Touch the Main Scene!)
*   **The Problem:** Unity scene files (`.unity`) are huge lists of computer code. If two people change things in the same scene at the same time, Git **cannot** merge them. One person's work will be completely deleted.
*   **The Rule:** **NEVER work directly in the main/master gameplay scene.** 
*   **The Solution:** 
    1. Go to `Assets/_Project/Scenes/` and find your designated sub-folder (Art, Code, Sound).
    2. Create your own personal scene (e.g., `Scene_Art_YourName` or `Scene_Code_Movement`).
    3. Build your assets, scripts, or layouts in **your scene only**.

### RULE 2: Prefabbing (How we put the game together)
*   **What is a Prefab?** Think of a Prefab as a "Lego Set" you save in your folders. It contains your art, code, and physics components all packaged up.
*   **The Workflow:**
    1. Build your player character, fuel cell, or obstacle in **your scene**.
    2. Drag that object from your Hierarchy window into the `Assets/_Project/Prefabs/` folder. It turns blue. It is now a **Prefab**.
    3. Commit and push your Prefab.
    4. The programmer/lead will drag your blue Prefab into the main scene. This way, we update the game without anyone ever touching the main scene!
    5. **Why this is awesome:** If you edit your Prefab, it will automatically update in the master scene without you having to open it!

### RULE 3: Do NOT Import "Trash" Assets
*   **The Problem:** If you download a 1GB asset pack from the internet but only use *one single rock sprite*, pushing the whole 1GB pack will slow down Git for everyone and make the repository explode.
*   **The Rule:** Only import or keep assets you are **actually using**. 
*   **How to do it:**
    *   If you download an asset pack, only drag the specific image, sound, or script you need into your `Assets/_Project/` folder.
    *   Delete unused test files before you commit.
    *   **How to not commit them:** If you imported something by accident, delete it from Unity's project window *before* you open GitHub Desktop.

---

## 📦 How to Use GitHub Desktop Safely (Daily Workflow)

Follow these steps **every single time** you sit down to work:

1.  **Before you open Unity:** Open GitHub Desktop.
    *   Make sure you are on **your designated branch** (e.g., `feature/art`).
    *   Click **Fetch Origin** in the top bar. If there are updates, click **Pull**. (This downloads other people's work).
2.  **Do your work in Unity:**
    *   Work in your scene.
    *   Save frequently (`Ctrl + S` or `Cmd + S`).
3.  **When you are done:** Go back to GitHub Desktop.
    *   Look at your "Changes" list.
    *   **Uncheck** any files that look like trash or things you didn't mean to change.
    *   Write a summary (e.g., `"Added astronaut movement sprite"`).
    *   Click **Commit to [Branch Name]**.
    *   Click **Push origin** to upload your work to the team.

---

## 📖 Beginners' Git Dictionary

*   **Repository (Repo):** The master folder where the whole game is stored online.
*   **Branch:** Your personal copy of the game. Working here means you can't break the main game.
*   **Commit:** A "Save Point." Doing a commit stamps your changes into history so you can always go back if something breaks.
*   **Push:** Uploading your Save Points to the online repository.
*   **Pull:** Downloading the latest Save Points made by your teammates.
*   **Stash:** A "pause button." If you have unfinished work but need to switch branches, GitHub Desktop will ask to "Stash" your changes (holding them in a temporary drawer).
*   **Conflict:** When two people edited the same file. If this happens, **STOP and message the team chat.** Do not try to force it.

---

## 🛠️ NEVER Do These Things!
*   ❌ **Never move or rename files outside of Unity:** If you rename a file using Windows Explorer or Mac Finder, Unity loses the connection (broken `.meta` files). Always drag, rename, and organize folders **inside Unity's Project Window**.
*   ❌ **Never Revert/Discard unless you are 100% sure:** If you discard changes in GitHub Desktop, they are gone forever. Ask a teammate first!