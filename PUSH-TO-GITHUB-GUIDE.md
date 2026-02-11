# Complete Beginner's Guide to Pushing to GitHub

## 📦 What You Have

You should have downloaded a ZIP file with these files:
```
ContextMenuInstaller/
├── .github/
│   └── workflows/
│       └── build.yml          (GitHub Actions - auto builds your .exe)
├── .gitignore                 (Tells git what files to ignore)
├── Program.cs                 (Your C# code - renamed from ContextMenuInstaller.cs)
├── ContextMenuInstaller.csproj (Project file for single-file build)
├── app.manifest               (Admin privileges config)
├── README.md                  (Project description for GitHub)
└── PUSH-TO-GITHUB-GUIDE.md    (This file)
```

## 🎯 What Will Happen

1. You'll upload these files to GitHub
2. GitHub will automatically build the .exe for you (takes ~2-3 minutes)
3. You'll download the .exe from GitHub
4. Share it with users!

No software installation needed on your laptop!

---

## 📝 Step-by-Step Instructions

### STEP 1: Create a GitHub Account (if you don't have one)

1. Go to https://github.com
2. Click "Sign up"
3. Follow the steps to create an account
4. Verify your email

**Skip this if you already have a GitHub account!**

---

### STEP 2: Create a New Repository

1. **Log in to GitHub**
2. **Click the "+" button** in the top-right corner
3. **Select "New repository"**

4. **Fill in the details:**
   - **Repository name:** `context-menu-installer` (or whatever you want)
   - **Description:** "Windows 11 Context Menu Installer Tool"
   - **Public or Private:** Choose "Public" (so GitHub Actions works for free)
   - **⚠️ IMPORTANT:** Do NOT check "Add a README file"
   - **⚠️ IMPORTANT:** Do NOT add .gitignore or license yet
   
5. **Click "Create repository"**

You'll see a page with instructions - **don't worry about those yet!**

---

### STEP 3: Upload Your Files to GitHub

There are two ways to do this. Choose the easiest for you:

#### METHOD A: Using GitHub Website (Easiest - No Software Needed!)

1. **On your new repository page, click "uploading an existing file"**
   (You'll see this link in the instructions GitHub shows you)

2. **Drag ALL your files into the upload area**
   - Make sure to include the `.github` folder!
   - You can drag the whole unzipped folder

3. **Important: Preserve folder structure**
   - GitHub might flatten folders
   - Make sure `.github/workflows/build.yml` stays in that path
   - If folders get flattened, upload them separately

4. **At the bottom, there's a "Commit changes" section:**
   - In the text box, type: `Initial commit`
   - Click the green **"Commit changes"** button

5. **Your files are now on GitHub!**

#### METHOD B: Using Git Desktop (If you prefer a visual tool)

1. **Download GitHub Desktop:**
   - Go to https://desktop.github.com
   - Download and install it
   - Sign in with your GitHub account

2. **Clone your repository:**
   - In GitHub Desktop, click "File" → "Clone repository"
   - Find your `context-menu-installer` repository
   - Choose where to save it on your laptop
   - Click "Clone"

3. **Copy your files:**
   - Open the folder where GitHub Desktop cloned the repository
   - Copy ALL your project files into this folder
   - Make sure the `.github` folder is included

4. **Commit and push:**
   - Go back to GitHub Desktop
   - You'll see all your files listed on the left
   - In the bottom-left, type a commit message: `Initial commit`
   - Click **"Commit to main"**
   - Click **"Push origin"** (top-right)

5. **Your files are now on GitHub!**

---

### STEP 4: Trigger the Build (Make GitHub Create Your .exe)

Now we need to tell GitHub to build the .exe. There are two ways:

#### OPTION A: Create a Release Tag (Recommended)

1. **Go to your repository on GitHub.com**
2. **Click "Releases"** (right side of the page)
3. **Click "Create a new release"**
4. **Click "Choose a tag"**
5. **Type:** `v1.0.0` (this is your version number)
6. **Click "Create new tag: v1.0.0 on publish"**
7. **Release title:** `Version 1.0.0`
8. **Description:** Type something like:
   ```
   First release of Context Menu Installer!
   
   Download the .exe file below to get started.
   ```
9. **Click "Publish release"**

**GitHub will now automatically start building your .exe!**

#### OPTION B: Manual Trigger (Alternative)

1. **Go to your repository on GitHub.com**
2. **Click the "Actions" tab** (top of page)
3. **Click "Build and Release"** workflow on the left
4. **Click "Run workflow"** (right side, might be a dropdown)
5. **Click the green "Run workflow"** button

---

### STEP 5: Download Your Built .exe

1. **Wait 2-3 minutes** for the build to complete
   - You can watch the progress in the "Actions" tab
   - A green checkmark ✓ means success
   - A red X means there was an error

2. **Once complete, go to the "Releases" page**
   - Click "Releases" on the right side of your repo

3. **You'll see your release (v1.0.0)**
   - Scroll down to "Assets"
   - **Click on `ContextMenuInstaller.exe` to download it**

4. **That's your .exe!** 
   - Ready to share with users
   - Test it on your laptop first

---

### STEP 6: Test Your .exe

1. **Download the .exe** from GitHub Releases
2. **Right-click it** → "Run as administrator"
3. **Test it works:**
   - Try option 1 (Install) with a JSON from your web app
   - Check if menu items appear
   - Try option 2 (Remove) to clean up

4. **If it works - you're done! 🎉**

---

## 🔄 Updating Your .exe (Future Releases)

When you need to release a new version:

1. **Make changes to your code** (if needed)
2. **Upload changed files** to GitHub (same as Step 3)
3. **Create a new release** with a new tag like `v1.0.1`, `v1.1.0`, etc.
4. **GitHub automatically builds the new .exe**
5. **Download and share the updated version**

---

## 🐛 Troubleshooting

### Problem: Build Failed (Red X in Actions)

1. **Go to Actions tab**
2. **Click on the failed workflow**
3. **Click on the "build" job**
4. **Look for red error messages**

Common issues:
- **Missing files:** Make sure ALL files uploaded, especially `.csproj` and `Program.cs`
- **Wrong folder structure:** `.github/workflows/build.yml` must be in that exact path
- **Wrong framework:** The `.csproj` should have `net8.0-windows`

**Solution:** Re-upload the files making sure the folder structure is correct

### Problem: Can't Find the .exe

**If using OPTION A (Release Tag):**
- Go to "Releases" page
- Look under "Assets" section
- Download `ContextMenuInstaller.exe`

**If using OPTION B (Manual Trigger):**
- Go to "Actions" tab
- Click on the completed workflow
- Scroll down to "Artifacts" section
- Download the zip file
- Extract to get the .exe

### Problem: Files Won't Upload to GitHub

- Make sure you're logged in to GitHub
- Try using GitHub Desktop instead of the website
- Check your internet connection
- Try uploading files one at a time

### Problem: .github Folder Not Uploading

The `.github` folder might be hidden on your computer:

**Windows:**
1. Open File Explorer
2. Go to "View" tab
3. Check "Hidden items"
4. Now you can see and upload the `.github` folder

**Or just upload via GitHub Desktop** - it handles hidden folders automatically

---

## 📚 What Each File Does (For Learning)

- **Program.cs** - Your C# source code
- **ContextMenuInstaller.csproj** - Tells .NET how to build your project
- **app.manifest** - Requests admin privileges when running
- **.github/workflows/build.yml** - Instructions for GitHub to build your .exe
- **.gitignore** - Tells git to ignore temporary build files
- **README.md** - Description shown on your GitHub repository page

---

## 🎓 Git/GitHub Terms Explained

- **Repository (Repo):** A folder on GitHub containing your project
- **Commit:** Saving changes with a description of what you changed
- **Push:** Uploading your commits from your computer to GitHub
- **Clone:** Downloading a repository from GitHub to your computer
- **Release:** A specific version of your project with downloadable files
- **Tag:** A label for a specific version (like v1.0.0)
- **Actions:** GitHub's automation system that builds your .exe
- **Workflow:** A set of automated steps (like building and releasing)

---

## ✅ Quick Checklist

Before you start:
- [ ] Extract the ZIP file I gave you
- [ ] Have a GitHub account (or create one)
- [ ] Know what you want to name your repository

Steps:
- [ ] Create new repository on GitHub
- [ ] Upload all files (keep folder structure!)
- [ ] Create a release with tag v1.0.0
- [ ] Wait for build to complete (2-3 min)
- [ ] Download .exe from Releases page
- [ ] Test the .exe
- [ ] Share with users!

---

## 🆘 Still Stuck?

If you get stuck at any step:

1. **Take a screenshot** of where you're stuck
2. **Check the Actions tab** on GitHub for error messages
3. **Double-check** you uploaded ALL files including `.github` folder
4. **Try GitHub Desktop** if website upload isn't working

Most issues are:
- Missing files (especially `.github/workflows/build.yml`)
- Wrong folder structure
- Not creating a release tag

---

## 🎉 Success!

Once you see the green checkmark in Actions and can download your .exe from Releases, you're done!

You can now:
- ✅ Share the download link with users
- ✅ Add it to your web app
- ✅ Update it anytime by creating new releases

**Congratulations on your first GitHub release!** 🎊

---

**Next:** Once this works, check out the main README.md for how to integrate the download link into your Spark web app!
