import re, json, shutil, sys

EP = r"D:\MKM-APK\ripped\ExportedProject"
ed = lambda p: open(p, encoding="utf-8-sig").read()

# 1. manifest: add pixel-perfect
mp = EP + r"\Packages\manifest.json"
m = json.loads(ed(mp))
m["dependencies"]["com.unity.2d.pixel-perfect"] = "1.0.1"
json.dump(m, open(mp, "w"), indent=2)
print("manifest: pixel-perfect added")

# 2. build script (EditorBuildSettings order version)
build_cs = open(r"D:\MKM-APK\build\BuildAndroid.cs", encoding="utf-8").read()
build_cs = build_cs.replace(
"""        string[] guids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
        string[] paths = guids.Select(g => AssetDatabase.GUIDToAssetPath(g)).ToArray();
        var ordered = paths.OrderBy(p => p).ToList();
        string boot = ordered.FirstOrDefault(p => p.EndsWith("level0.unity")) ?? ordered.First();
        ordered.Remove(boot);
        ordered.Insert(0, boot);
        Debug.Log("MKM scenes:\\n" + string.Join("\\n", ordered));""",
"""        string[] ordered = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
        Debug.Log("MKM scenes:\\n" + string.Join("\\n", ordered));""")
open(EP + r"\Assets\Editor\MKMBuild.cs", "w").write(build_cs)
print("MKMBuild.cs written")

# 3. P0: UserData -> persistentDataPath in GameCore.cs
gcp = EP + r"\Assets\Scripts\Assembly-CSharp\GameCore.cs"
gc = ed(gcp)
n_userdata = gc.count('"UserData"')
assert n_userdata >= 10, f"expected >=10 UserData sites, found {n_userdata}"
gc = gc.replace('"UserData"', 'MKMSaveDir()')
helper = ("\n\tprivate static string MKMSaveDir()\n\t{\n"
          '\t\tstring dir = System.IO.Path.Combine(Application.persistentDataPath, "UserData");\n'
          "\t\tif (!System.IO.Directory.Exists(dir))\n\t\t{\n"
          "\t\t\tSystem.IO.Directory.CreateDirectory(dir);\n\t\t}\n"
          "\t\treturn dir;\n\t}\n")
anchor = "private bool DataDirectoryCheck()"
assert anchor in gc, "DataDirectoryCheck anchor missing"
gc = gc.replace(anchor, helper + "\n\t" + anchor, 1)

# 4. P1a: WEBGL check += Android
pat = r"if\s*\(\s*_gameBuild\s*==\s*GameBuildType\.WebGL\s*\)"
gc2, n = re.subn(pat, "if (_gameBuild == GameBuildType.WebGL || Application.platform == RuntimePlatform.Android)", gc)
assert n == 1, f"WEBGL check patched {n} times"
open(gcp, "w", encoding="utf-8").write(gc2)
print(f"P0: {n_userdata} UserData sites + helper; P1a done")

# 5. P1b: SplashScreen feed condition += Android
ssp = EP + r"\Assets\Scripts\Assembly-CSharp\SplashScreen.cs"
ss = ed(ssp)
old = "if (GC.GameBuildCurrent == GameCore.GameBuildType.WebGL || GC.GameBuildCurrent == GameCore.GameBuildType.Mac)"
assert old in ss, "splash condition not found"
ss = ss.replace(old, old[:-1] + " || Application.platform == RuntimePlatform.Android)", 1)
open(ssp, "w", encoding="utf-8").write(ss)
print("P1b done")
print("ALL PATCHES OK")
