﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OsmSharp.Osm.Data.XML.Raw.Processor;
using OsmSharp.Osm.Data.Core.Processor.Filter.Sort;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OsmSharp.Routing.CH.PreProcessing.Ordering;
using OsmSharp.Routing.CH.PreProcessing;
using OsmSharp.Routing.CH.PreProcessing.Witnesses;
using OsmSharp.Osm.Core;
using OsmSharp.Osm.Routing.Data.Processing;
using OsmSharp.Routing.Core.Interpreter;
using OsmSharp.Routing.Core.Graph.Memory;

namespace OsmSharp.Osm.UnitTests.Routing.CH
{
    /// <summary>
    /// Tests the EdgeDifference calculator.
    /// </summary>
    [TestClass]
    public class CHEdgeDifferenceTests
    {
        /// <summary>
        /// Builds the data source.
        /// </summary>
        /// <returns></returns>
        private MemoryRouterDataSource<CHEdgeData> BuildData(IRoutingInterpreter interpreter)
        {
            OsmTagsIndex tags_index = new OsmTagsIndex();

            // do the data processing.
            MemoryRouterDataSource<CHEdgeData> data =
                new MemoryRouterDataSource<CHEdgeData>(tags_index);
            CHEdgeDataGraphProcessingTarget target_data = new CHEdgeDataGraphProcessingTarget(
                data, interpreter, data.TagsIndex);
            XmlDataProcessorSource data_processor_source = new XmlDataProcessorSource(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("OsmSharp.UnitTests.test_network.osm"));
            DataProcessorFilterSort sorter = new DataProcessorFilterSort();
            sorter.RegisterSource(data_processor_source);
            target_data.RegisterSource(sorter);
            target_data.Pull();

            return data;
        }

        /// <summary>
        /// Builds the edge difference.
        /// </summary>
        private EdgeDifference BuildEdgeDifference(IRoutingInterpreter interpreter)
        {
            MemoryRouterDataSource<CHEdgeData> data = this.BuildData(interpreter);

            // do the pre-processing part.
            INodeWitnessCalculator witness_calculator = new DykstraWitnessCalculator(data);
            return new EdgeDifference(
                data, witness_calculator);
        }

        /// <summary>
        /// Builds the edge difference.
        /// </summary>
        private CHPreProcessor BuildCHPreProcessor(IRoutingInterpreter interpreter)
        {
            MemoryRouterDataSource<CHEdgeData> data = this.BuildData(interpreter);

            // do the pre-processing part.
            INodeWitnessCalculator witness_calculator = new DykstraWitnessCalculator(data);
            EdgeDifference edge_difference = new EdgeDifference(
                data, witness_calculator);

            CHPreProcessor pre_processor = new CHPreProcessor(
                data, edge_difference, witness_calculator);
            return pre_processor;
        }

        /// <summary>
        /// Tests all the edge difference calculations on the uncontracted test network.
        /// </summary>
        [TestMethod]
        public void TestCHEdgeDifferenceNonContracted()
        {
            IRoutingInterpreter interpreter = new OsmSharp.Osm.Routing.Interpreter.OsmRoutingInterpreter();
            EdgeDifference edge_difference = this.BuildEdgeDifference(interpreter);

            Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
            Assert.AreEqual(6, edge_difference.Calculate(2));
            Assert.AreEqual(0, edge_difference.Calculate(3));
            Assert.AreEqual(0, edge_difference.Calculate(4));
            Assert.AreEqual(0, edge_difference.Calculate(5));
            Assert.AreEqual(0, edge_difference.Calculate(6));
            Assert.AreEqual(3, edge_difference.Calculate(7));
            Assert.AreEqual(3, edge_difference.Calculate(8));
            Assert.AreEqual(0, edge_difference.Calculate(9));
            Assert.AreEqual(3, edge_difference.Calculate(10));
            Assert.AreEqual(3, edge_difference.Calculate(11));
            Assert.AreEqual(0, edge_difference.Calculate(12));
            Assert.AreEqual(0, edge_difference.Calculate(13));
            Assert.AreEqual(0, edge_difference.Calculate(14));
            Assert.AreEqual(0, edge_difference.Calculate(15));
            Assert.AreEqual(3, edge_difference.Calculate(16));
            Assert.AreEqual(0, edge_difference.Calculate(17));
            Assert.AreEqual(0, edge_difference.Calculate(18));
            Assert.AreEqual(0, edge_difference.Calculate(19));
            Assert.AreEqual(-1, edge_difference.Calculate(20));
            Assert.AreEqual(0, edge_difference.Calculate(21));
            Assert.AreEqual(0, edge_difference.Calculate(22));
            Assert.AreEqual(-1, edge_difference.Calculate(23));
        }

        ///// <summary>
        ///// Tests all the edge difference calculations on the uncontracted test network.
        ///// </summary>
        //[TestMethod]
        //public void TestCHEdgeDifferenceContractions()
        //{
        //    CHPreProcessor processor = this.BuildCHPreProcessor();
        //    processor.InitializeQueue();
        //    INodeWeightCalculator edge_difference = processor.NodeWeightCalculator;

        //    Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(6, edge_difference.Calculate(2));
        //    Assert.AreEqual(0, edge_difference.Calculate(3));
        //    Assert.AreEqual(0, edge_difference.Calculate(4));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(3, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(3, edge_difference.Calculate(16));
        //    Assert.AreEqual(0, edge_difference.Calculate(17));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));
        //    Assert.AreEqual(-1, edge_difference.Calculate(20));
        //    Assert.AreEqual(0, edge_difference.Calculate(21));
        //    Assert.AreEqual(0, edge_difference.Calculate(22));
        //    Assert.AreEqual(-1, edge_difference.Calculate(23));

        //    // contract 20.
        //    processor.Contract(20);

        //    Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(6, edge_difference.Calculate(2));
        //    Assert.AreEqual(0, edge_difference.Calculate(3));
        //    Assert.AreEqual(0, edge_difference.Calculate(4));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(3, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(3, edge_difference.Calculate(16));
        //    Assert.AreEqual(0, edge_difference.Calculate(17));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));
        //    Assert.AreEqual(-1, edge_difference.Calculate(21));
        //    Assert.AreEqual(0, edge_difference.Calculate(22));
        //    Assert.AreEqual(-1, edge_difference.Calculate(23));

        //    // contract 21.
        //    processor.Contract(21);

        //    Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(6, edge_difference.Calculate(2));
        //    Assert.AreEqual(0, edge_difference.Calculate(3));
        //    Assert.AreEqual(0, edge_difference.Calculate(4));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(3, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(16));
        //    Assert.AreEqual(0, edge_difference.Calculate(17));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));
        //    Assert.AreEqual(0, edge_difference.Calculate(22));
        //    Assert.AreEqual(-1, edge_difference.Calculate(23));

        //    // contract 23.
        //    processor.Contract(23);

        //    Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(6, edge_difference.Calculate(2));
        //    Assert.AreEqual(0, edge_difference.Calculate(3));
        //    Assert.AreEqual(0, edge_difference.Calculate(4));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(3, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(16));
        //    Assert.AreEqual(0, edge_difference.Calculate(17));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));
        //    Assert.AreEqual(-1, edge_difference.Calculate(22));

        //    // contract 22.
        //    processor.Contract(22);

        //    Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(6, edge_difference.Calculate(2));
        //    Assert.AreEqual(0, edge_difference.Calculate(3));
        //    Assert.AreEqual(0, edge_difference.Calculate(4));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(3, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(-1, edge_difference.Calculate(16));
        //    Assert.AreEqual(0, edge_difference.Calculate(17));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    // contract 16.
        //    processor.Contract(16);

        //    Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(6, edge_difference.Calculate(2));
        //    Assert.AreEqual(0, edge_difference.Calculate(3));
        //    Assert.AreEqual(0, edge_difference.Calculate(4));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(3, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(-1, edge_difference.Calculate(17));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    // contract 17.
        //    processor.Contract(17);

        //    Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(6, edge_difference.Calculate(2));
        //    Assert.AreEqual(0, edge_difference.Calculate(3));
        //    Assert.AreEqual(0, edge_difference.Calculate(4));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(0, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(3);

        //    Assert.AreEqual(1, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(6, edge_difference.Calculate(2));
        //    Assert.AreEqual(-2, edge_difference.Calculate(4));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(0, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(4);

        //    Assert.AreEqual(0, edge_difference.Calculate(1)); // witness paths from 2<->4.
        //    Assert.AreEqual(3, edge_difference.Calculate(2));
        //    Assert.AreEqual(0, edge_difference.Calculate(5));
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(0, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(1);

        //    Assert.AreEqual(1, edge_difference.Calculate(2)); // witness paths from 11<->5.
        //    Assert.AreEqual(-2, edge_difference.Calculate(5)); // witness paths from 11<->6.
        //    Assert.AreEqual(0, edge_difference.Calculate(6));
        //    Assert.AreEqual(0, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(5);

        //    Assert.AreEqual(0, edge_difference.Calculate(2)); // witness paths from 11<->5.
        //    Assert.AreEqual(-1, edge_difference.Calculate(6));
        //    Assert.AreEqual(0, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(6);

        //    Assert.AreEqual(0, edge_difference.Calculate(2)); // witness paths from 11<->5.
        //    Assert.AreEqual(-1, edge_difference.Calculate(7));
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(3, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(7);

        //    Assert.AreEqual(0, edge_difference.Calculate(2)); // witness paths from 11<->5.
        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(0, edge_difference.Calculate(11));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(2);

        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(3, edge_difference.Calculate(10));
        //    Assert.AreEqual(-2, edge_difference.Calculate(11)); // witness paths from 18<->10.
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(18));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(11);

        //    Assert.AreEqual(3, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(0, edge_difference.Calculate(10));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(-1, edge_difference.Calculate(18)); // witness paths from 10<->8.
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(18);

        //    Assert.AreEqual(0, edge_difference.Calculate(8));
        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(0, edge_difference.Calculate(10));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(8);

        //    Assert.AreEqual(0, edge_difference.Calculate(9));
        //    Assert.AreEqual(0, edge_difference.Calculate(10));
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(9);

        //    Assert.AreEqual(-2, edge_difference.Calculate(10)); // witness paths from 19<->12.
        //    Assert.AreEqual(0, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(-2, edge_difference.Calculate(19)); // witness paths from 15<->10.

        //    processor.Contract(10);

        //    Assert.AreEqual(-1, edge_difference.Calculate(12));
        //    Assert.AreEqual(0, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(-1, edge_difference.Calculate(19)); // witness paths from 15<->10.

        //    processor.Contract(12);

        //    Assert.AreEqual(-1, edge_difference.Calculate(13));
        //    Assert.AreEqual(0, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(-1, edge_difference.Calculate(19)); // witness paths from 15<->10.

        //    processor.Contract(13);

        //    Assert.AreEqual(-1, edge_difference.Calculate(14));
        //    Assert.AreEqual(0, edge_difference.Calculate(15));
        //    Assert.AreEqual(-1, edge_difference.Calculate(19)); // witness paths from 15<->10.

        //    processor.Contract(14);

        //    Assert.AreEqual(-1, edge_difference.Calculate(15));
        //    Assert.AreEqual(-1, edge_difference.Calculate(19)); // witness paths from 15<->10.

        //    processor.Contract(15);

        //    Assert.AreEqual(0, edge_difference.Calculate(19));

        //    processor.Contract(19);
        //}
    }
}