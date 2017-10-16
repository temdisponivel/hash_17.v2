using HASH;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Executes the dir program.
    /// Dir lists the contents of a directory.
    /// </summary>
    public static class DirProgram
    {
        /// <summary>
        /// Executes the dir program.
        /// </summary>
        public static void Execute(ProgramExecutionOptions option)
        {
            var currentFolder = Global.FileSystemData.CurrentDir;
            for (int i = 0; i < currentFolder.Childs.Count; i++)
            {
                Debug.Log(currentFolder.Childs[i].Name);
            }
        }
    }
}