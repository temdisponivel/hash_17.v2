using UnityEngine;

namespace HASH
{
    public static class PrintProgram
    {
        public const string PrintFolderPath = "/prints";
        public const string PrintFolderName = "prints";
        
        public static void Execute(ProgramExecutionOptions options)
        {
            var width = Screen.currentResolution.width;
            var height = Screen.currentResolution.height;
            var result = new Texture2D(width, height, TextureFormat.RGB24, false);
            var targetRender = new RenderTexture(width, height, 24);
            
            Camera.main.targetTexture = targetRender;
            Camera.main.Render();
            
            RenderTexture.active = targetRender;
            
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            result.Apply();

            RenderTexture.active = null;
            Camera.main.targetTexture = null;
            
            targetRender.Release();
            Object.Destroy(targetRender);

            var printsFolder = FileSystem.FindDirByPath(PrintFolderPath);
            if (printsFolder == null)
            {
                var rootFolder = FileSystem.GetRootDir();
                printsFolder = FileSystem.CreateDir(rootFolder, PrintFolderName);
            }

            var file = FileSystem.CreateImageFile(printsFolder, "print", result);

            var msg = string.Format("Printscreen saved to '{0}'.", file.FullPath);
            msg = TextUtil.Success(msg);
            TerminalUtil.ShowText(msg);
            
            OpenProgram.OpenImageFile(file, file.Content as ImageFile, false);
        }
    }
}