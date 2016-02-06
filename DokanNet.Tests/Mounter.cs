﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace DokanNet.Tests
{
    [TestClass]
    public static class Mounter
    {
        private static Thread mounterThread;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
#if NETWORK_DRIVE
            (mounterThread = new Thread(new ThreadStart(() => DokanOperationsFixture.Operations.Mount(DokanOperationsFixture.MOUNT_POINT, DokanOptions.DebugMode | DokanOptions.NetworkDrive, 5)))).Start();
#else
            (mounterThread = new Thread(new ThreadStart(() => DokanOperationsFixture.Operations.Mount(DokanOperationsFixture.MOUNT_POINT, DokanOptions.DebugMode | DokanOptions.RemovableDrive, 5)))).Start();
#endif
            var drive = new DriveInfo(DokanOperationsFixture.MOUNT_POINT);
            while (!drive.IsReady)
                Thread.Sleep(50);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            mounterThread.Abort();
            Dokan.Unmount(DokanOperationsFixture.MOUNT_POINT[0]);
            Dokan.RemoveMountPoint(DokanOperationsFixture.MOUNT_POINT.ToString(CultureInfo.InvariantCulture));
        }
    }
}
