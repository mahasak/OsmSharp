﻿// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2013 Abelshausen Ben
// 
// This file is part of OsmSharp.
// 
// OsmSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// OsmSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with OsmSharp. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using OsmSharp.UI.Renderer.Scene;
using OsmSharp.Collections.Tags;

namespace OsmSharp.Test.Performance.UI.Scene
{
    /// <summary>
    /// Contains Scene2D tests.
    /// </summary>
    public static class Scene2DTests
    {
        /// <summary>
        /// Tests serializing a stream.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="outputFile"></param>
        /// <param name="scene"></param>
        /// <param name="compress"></param>
        public static Stream TestSerialize(string name, string outputFile, Scene2D scene, bool compress)
        {
            FileInfo testFile = new FileInfo(string.Format(@".\TestFiles\{0}", outputFile));
            testFile.Delete();

            Stream stream = testFile.OpenWrite();

            Scene2DTests.TestSerialize(name, stream, scene, compress);

            stream.Dispose();

            OsmSharp.Logging.Log.TraceEvent(name, OsmSharp.Logging.TraceEventType.Information,
                string.Format("Serialized file: {0}B", testFile.Length));

            return testFile.OpenRead();
        }

        /// <summary>
        /// Tests serializing a stream.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        /// <param name="scene"></param>
        /// <param name="compress"></param>
        public static void TestSerialize(string name, Stream stream, Scene2D scene, bool compress)
        {
            PerformanceInfoConsumer performanceInfo = new PerformanceInfoConsumer(string.Format("{0}.Serialize", name));
            performanceInfo.Start();
            performanceInfo.Report("Serializing stream...");

            TagsCollectionBase metaTags = new TagsCollection();
            metaTags.Add("generated_by", "performance_test");
            scene.Serialize(stream, compress, metaTags);

            performanceInfo.Stop();

            Console.Write("", scene.BackColor);
        }
    }
}