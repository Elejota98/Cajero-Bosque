using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Ds.BusinessObjects.Entities;

namespace Ds.Utilidades
{
    public static class InstalledPrograms
    {
        const string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public static bool UnistallProgram(string sNombre)
        {
            bool bEstado = true;
            MyRegistro registro = InstalledPrograms.GetRegistryProgram(sNombre);
            if (registro != null)
            {
                Console.WriteLine(registro.Name + " " + registro.Version + " " + registro.UnistallKey);

                string uninstall = registro.UnistallKey;

                System.Console.WriteLine(uninstall);
                System.Diagnostics.Process FProcess = new System.Diagnostics.Process();
                string temp = "/x{" + uninstall.Split("/".ToCharArray())[1].Split("I{".ToCharArray())[2];
                //replacing with /x with /i would cause another popup of the application uninstall
                FProcess.StartInfo.FileName = uninstall.Split("/".ToCharArray())[0];
                FProcess.StartInfo.Arguments = temp;
                FProcess.StartInfo.UseShellExecute = false;
                FProcess.Start();

            }
            else
            {
                bEstado = false;
                Console.WriteLine("Registro no encontrado");
            }
            return bEstado;
        }

        public static MyRegistro GetRegistryProgram(string sDisplayName)
        {
            MyRegistro registro = null;

            registro = GetRegistrydProgramsFromRegistry(RegistryView.Registry32, sDisplayName);

            if (registro != null)
            {
                return registro;
            }
            else
            {
                registro = GetRegistrydProgramsFromRegistry(RegistryView.Registry64, sDisplayName);
            }


            return registro;
        }

        private static MyRegistro GetRegistrydProgramsFromRegistry(RegistryView registryView, string sDisplayName)
        {
            MyRegistro resgistro = new MyRegistro();

            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView).OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        if (IsProgramVisible(subkey))
                        {
                            if ((string)subkey.GetValue("DisplayName") == sDisplayName)
                            {
                                resgistro.Name = (string)subkey.GetValue("DisplayName");
                                resgistro.Version = (string)subkey.GetValue("DisplayVersion");
                                resgistro.UnistallKey = (string)subkey.GetValue("UninstallString");
                                return resgistro;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static bool IsProgramVisible(RegistryKey subkey)
        {
            var name = (string)subkey.GetValue("DisplayName");
            var releaseType = (string)subkey.GetValue("ReleaseType");
            //var unistallString = (string)subkey.GetValue("UninstallString");
            var systemComponent = subkey.GetValue("SystemComponent");
            var parentName = (string)subkey.GetValue("ParentDisplayName");

            return
                !string.IsNullOrEmpty(name)
                && string.IsNullOrEmpty(releaseType)
                && string.IsNullOrEmpty(parentName)
                && (systemComponent == null);
        }
    }
}
