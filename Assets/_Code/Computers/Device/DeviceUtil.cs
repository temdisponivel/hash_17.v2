using System;
using HASH.Story;
using SimpleCollections.Lists;
using UnityEngine;
using UnityEngine.iOS;

namespace HASH
{
    public static class DeviceUtil
    {
        public const string DEFAULT_USER_NAME = "guest";
        public const string DEFAULT_PASSWORD = "guest";

        public static bool TryFindeDeviceByName(string name, out HashDevice device)
        {
            device = FindDeviceByName(name);
            return device != null;
        }

        public static HashDevice FindDeviceByName(string name)
        {
            return InternalFindDeviceByName(DataHolder.DeviceData, name);
        }

        public static bool TryFindDeviceByIp(string ip, out HashDevice device)
        {
            device = FindDeviceByIp(ip);
            return device != null;
        }

        public static HashDevice FindDeviceByIp(string ip)
        {
            var allDevices = DataHolder.DeviceData.AllDevices;
            for (int i = 0; i < allDevices.Count; i++)
            {
                var device = allDevices[i];
                if (string.Equals(device.IpAddress, ip, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (IsDeviceAvailable(device))
                        return device;
                }
            }

            return null;
        }

        public static HashDevice FindDeviceByIpOrName(string ipOrName)
        {
            var device = FindDeviceByIp(ipOrName);
            if (device == null)
                device = FindDeviceByName(ipOrName);
            return device;
        }

        private static HashDevice InternalFindDeviceByName(DeviceData data, string name)
        {
            var allDevices = data.AllDevices;
            for (int i = 0; i < allDevices.Count; i++)
            {
                var device = allDevices[i];
                if (string.Equals(device.DeviceName, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (IsDeviceAvailable(device))
                        return device;
                }
            }

            return null;
        }

        public static bool IsDeviceAvailable(HashDevice device)
        {
            var result = StoryUtil.EvaluateCondition(device.Condition);
            return result;
        }

        public static bool TryFindUserByName(HashDevice device, string userName, out HashUser user)
        {
            user = FindUserByName(device, userName);
            return user != null;
        }

        public static HashUser FindUserByName(HashDevice device, string userName)
        {
            var users = device.AllUsers;
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                if (string.Equals(user.Username, userName, StringComparison.InvariantCultureIgnoreCase))
                    return user;
            }

            return null;
        }

        public static bool TryLogin(HashDevice device, string userName, string password)
        {
            var user = FindUserByName(device, userName);
            if (user == null)
                return false;

            return AuthenticateUser(user, password);
        }

        public static bool AuthenticateUser(HashUser user, string passwordAttempt)
        {
            return string.Equals(user.Password, passwordAttempt);
        }

        public static void ChangeDevice(HashDevice newDevice, HashUser newUser)
        {
            DataHolder.DeviceData.CurrentDevice = newDevice;
            DataHolder.DeviceData.CurrentUser = newUser;

            UpdateDeviceRelatedGUI();
        }

        public static void UpdateDeviceRelatedGUI()
        {
            TerminalUtil.UpdateCurrentPathLabel();
            TerminalUtil.ChangeToAvailableCommandsBuffer();
            TerminalUtil.ResetCommandBufferIndex();
        }

        public static HashUser GetUserFromSerializedData(SerializedHashUser serializedUser)
        {
            DebugUtil.AssertContext(serializedUser == null, "Null user", serializedUser);

            var result = new HashUser();
            result.Username = serializedUser.UserName;
            result.Password = serializedUser.Password;
            return result;
        }

        public static HashDevice GetDeviceFromSerializedData(SerializedHashDevice serializedDevice)
        {
            var device = new HashDevice();
            device.DeviceName = serializedDevice.DeviceName;
            device.Condition = serializedDevice.Condition;
            device.IpAddress = serializedDevice.IpAddress;

            device.AllPrograms = ProgramUtil.GetAllProgramsFromSerializedData(serializedDevice.Programs);
            device.AllUsers = SList.Create<HashUser>(serializedDevice.Users.Length);

            for (int i = 0; i < serializedDevice.Users.Length; i++)
            {
                var user = GetUserFromSerializedData(serializedDevice.Users[i]);
                SList.Add(device.AllUsers, user);
            }

            var defaultUser = new HashUser();
            defaultUser.Username = DEFAULT_USER_NAME;
            defaultUser.Password = DEFAULT_PASSWORD;

            SList.Add(device.AllUsers, defaultUser);

            device.FileSystem = FileSystem.GetFileSystemFromSerializedData(serializedDevice.FileSystem);

            return device;
        }

        public static DeviceData GetDeviceDataFromSerializedData(SerializedHashDevices devices)
        {
            var result = new DeviceData();
            result.AllDevices = SList.Create<HashDevice>(devices.Devices.Length);
            for (int i = 0; i < devices.Devices.Length; i++)
            {
                var serializedDevice = devices.Devices[i];
                var device = GetDeviceFromSerializedData(serializedDevice);
                SList.Add(result.AllDevices, device);
            }

            result.PlayerDevice = InternalFindDeviceByName(result, devices.PlayerDeviceName);
            result.CurrentUser = FindUserByName(result.PlayerDevice, devices.PlayerUserName);

            result.CurrentDevice = result.PlayerDevice;

            return result;
        }

        public static bool HasProgram(HashDevice device, ProgramType programType)
        {
            var programs = ProgramUtil.GetAvailablePrograms(device);
            return SList.Exists(programs, p => p.ProgramType == programType);
        }
    }
}