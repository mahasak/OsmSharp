﻿// OsmSharp - OpenStreetMap tools & library.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OsmSharp.Osm;
using OsmSharp.Collections.Tags;
using OsmSharp.UI.Map.Styles.MapCSS;
using OsmSharp.UI.Renderer;
using OsmSharp.Math.Geo.Projections;
using OsmSharp.Osm.Data.Memory;
using OsmSharp.UI.Renderer.Scene2DPrimitives;

namespace OsmSharp.UI.Unittests.Map.Styles.MapCSS.v0_2
{
    /// <summary>
    /// Tests a few MapCSS interpretation as stated in:
    /// http://josm.openstreetmap.de/wiki/Help/Styles/MapCSSImplementation
    /// </summary>
    [TestFixture]
    public class MapCSSInterpretationTests
    {
        /// <summary>
        /// Tests an empty CSS file.
        /// 
        /// Default settings for OsmSharp are: 
        /// canvas {
        ///     background-color: black;
        ///     default-points: false;
        ///     default-lines: false;
        /// }
        /// </summary>
        [Test]
        public void TestEmptyCSS()
        {
            // create 'test' objects.
            Node node1 = new Node();
            node1.Id = 1;
            node1.Latitude = 1;
            node1.Longitude = 1;

            Node node2 = new Node();
            node2.Id = 2;
            node2.Latitude = 2;
            node2.Longitude = 2;

            Way way = new Way();
            way.Id = 1;
            way.Nodes = new List<long>();
            way.Nodes.Add(1);
            way.Nodes.Add(2);

            // create the datasource.
            MemoryDataSource dataSource = new MemoryDataSource();
            dataSource.AddNode(node1);
            dataSource.AddNode(node2);
            dataSource.AddWay(way);

            // create the projection and scene objects.
            var mercator = new WebMercator();
            Scene2D scene = new Scene2D();

            // create the interpreter.
            MapCSSInterpreter interpreter = new MapCSSInterpreter(string.Empty,
                new MapCSSDictionaryImageSource());
            interpreter.Translate(scene, mercator, dataSource, node1);
            interpreter.Translate(scene, mercator, dataSource, node2);
            interpreter.Translate(scene, mercator, dataSource, way);

            // test the scene contents.
            Assert.AreEqual(0, scene.Count);
            Assert.AreEqual(SimpleColor.FromKnownColor(KnownColor.Black).Value, scene.BackColor);
        }

        /// <summary>
        /// Tests the canvas settings.
        /// </summary>
        [Test]
        public void TestCanvasSettingsCSS()
        {
            // create CSS.
            string css = "canvas { " +
                "fill-color: green; " +
                "} ";

            // create 'test' objects.
            Node node1 = new Node();
            node1.Id = 1;
            node1.Latitude = 1;
            node1.Longitude = 1;

            Node node2 = new Node();
            node2.Id = 2;
            node2.Latitude = 2;
            node2.Longitude = 2;

            Way way = new Way();
            way.Id = 1;
            way.Nodes = new List<long>();
            way.Nodes.Add(1);
            way.Nodes.Add(2);

            // create the datasource.
            MemoryDataSource dataSource = new MemoryDataSource();
            dataSource.AddNode(node1);
            dataSource.AddNode(node2);
            dataSource.AddWay(way);

            // create the projection and scene objects.
            var mercator = new WebMercator();
            Scene2D scene = new Scene2D();

            // create the interpreter.
            MapCSSInterpreter interpreter = new MapCSSInterpreter(css,
                new MapCSSDictionaryImageSource());
            interpreter.Translate(scene, mercator, dataSource, node1);
            interpreter.Translate(scene, mercator, dataSource, node2);
            interpreter.Translate(scene, mercator, dataSource, way);

            // test the scene contents.
            Assert.AreEqual(0, scene.Count);
            Assert.AreEqual(SimpleColor.FromKnownColor(KnownColor.Green).Value, scene.BackColor);
        }

        /// <summary>
        /// Tests the canvas settings.
        /// </summary>
        [Test]
        public void TestCanvasJOSMSettingsCSS()
        {
            // create CSS.
            string css = "canvas { " +
                "background-color: white; " +
                "default-points: true; " + // adds default points for every node (color: black, size: 2).
                "default-lines: true; " + // adds default lines for every way (color: red, width: 1).
                "} ";

            // create 'test' objects.
            Node node1 = new Node();
            node1.Id = 1;
            node1.Latitude = 1;
            node1.Longitude = 1;

            Node node2 = new Node();
            node2.Id = 2;
            node2.Latitude = 2;
            node2.Longitude = 2;

            Way way = new Way();
            way.Id = 1;
            way.Nodes = new List<long>();
            way.Nodes.Add(1);
            way.Nodes.Add(2);

            // create the datasource.
            MemoryDataSource dataSource = new MemoryDataSource();
            dataSource.AddNode(node1);
            dataSource.AddNode(node2);
            dataSource.AddWay(way);

            // create the projection and scene objects.
            var mercator = new WebMercator();
            Scene2D scene = new Scene2D();

            // create the interpreter.
            MapCSSInterpreter interpreter = new MapCSSInterpreter(css,
                new MapCSSDictionaryImageSource());
            interpreter.Translate(scene, mercator, dataSource, node1);
            interpreter.Translate(scene, mercator, dataSource, node2);
            interpreter.Translate(scene, mercator, dataSource, way);

            // test the scene contents.
            Assert.AreEqual(3, scene.Count);
            Assert.AreEqual(SimpleColor.FromKnownColor(KnownColor.White).Value, scene.BackColor);

            // test the scene point 1.
            IScene2DPrimitive primitive = scene.Get(0);
            Assert.IsNotNull(primitive);
            Assert.IsInstanceOf<Point2D>(primitive);
            Point2D point = primitive as Point2D;
            Assert.AreEqual(2, point.Size);
            Assert.AreEqual(SimpleColor.FromKnownColor(KnownColor.Black).Value, point.Color);
            Assert.AreEqual(mercator.LongitudeToX(1), point.X);
            Assert.AreEqual(mercator.LatitudeToY(1), point.Y);

            // test the scene point 2.
            primitive = scene.Get(1);
            Assert.IsNotNull(primitive);
            Assert.IsInstanceOf<Point2D>(primitive);
            point = primitive as Point2D;
            Assert.AreEqual(2, point.Size);
            Assert.AreEqual(SimpleColor.FromKnownColor(KnownColor.Black).Value, point.Color);
            Assert.AreEqual(mercator.LongitudeToX(2), point.X);
            Assert.AreEqual(mercator.LatitudeToY(2), point.Y);

            // test the scene line 2.
            primitive = scene.Get(2);
            Assert.IsNotNull(primitive);
            Assert.IsInstanceOf<Line2D>(primitive);
            Line2D line = primitive as Line2D;
            Assert.AreEqual(1, line.Width);
            Assert.AreEqual(SimpleColor.FromKnownColor(KnownColor.Red).Value, line.Color);
            Assert.IsNotNull(line.X);
            Assert.IsNotNull(line.Y);
            Assert.AreEqual(2, line.X.Length);
            Assert.AreEqual(2, line.Y.Length);
            Assert.AreEqual(mercator.LongitudeToX(1), line.X[0]);
            Assert.AreEqual(mercator.LatitudeToY(1), line.Y[0]);
            Assert.AreEqual(mercator.LongitudeToX(2), line.X[1]);
            Assert.AreEqual(mercator.LatitudeToY(2), line.Y[1]);
        }

        /// <summary>
        /// Does some tests to test the behaviour when using different layers.
        /// </summary>
        [Test]
        public void TestSimpleLayerRepresentation()
        {
            Node node = new Node();
            node.Id = 1;
            node.Latitude = 1;
            node.Longitude = 1;
            node.Tags = new SimpleTagsCollection();
            node.Tags.Add("name", "Wechel");
            node.Tags.Add("place", "City");


        }
    }
}
