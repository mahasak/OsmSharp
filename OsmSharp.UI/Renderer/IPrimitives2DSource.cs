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

using System.Collections.Generic;
using OsmSharp.UI.Renderer.Primitives;

namespace OsmSharp.UI.Renderer
{
    /// <summary>
    /// Abstract representation of a primitives source.
    /// </summary>
    public interface IPrimitives2DSource
    {
        /// <summary>
        /// Adds all primitives inside the given box for the given zoom.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="zoomFactor"></param>
        /// <returns></returns>
        IEnumerable<Primitive2D> Get(View2D view, float zoomFactor);

        /// <summary>
        /// Called when the current request has to be cancelled.
        /// </summary>
        void GetCancel();
    }
}