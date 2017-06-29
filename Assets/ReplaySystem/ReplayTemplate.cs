/*
 This file is part of Replay Framework for Unity .

    Mindset reader for Unity is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Foobar is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// This is a template for replay component.
/// <seealso cref="TransformReplay"/>
/// <seealso cref="ColorReplay"/>
/// </summary>
[RequireComponent(typeof(Replayable))]
public class ReplayTemplate : MonoBehaviour
{

	/// <summary>
	/// This method will be called when <see cref="Replayable"/> start recording. 
	/// </summary>
	public void RecordStarting ()
	{
	}

	/// <summary>
	/// This method will be called when <see cref="Replayable"/> stop recording. 
	/// </summary>
	public void RecordStopped ()
	{
	}


	/// <summary>
	/// This method will be called when <see cref="Replayable"/> start replaying. 
	/// </summary>
	public void ReplayStarting ()
	{
	}

	/// <summary>
	/// This method will be called when <see cref="Replayable"/> stop replaying. 
	/// </summary>
	public void ReplayStopped ()
	{
	}


	/// <summary>
	/// This method will be called when <see cref="ReplayManager"/> initilize <see cref="Replayable"/> components. 
	/// </summary>
	public void ReplayReset ()
	{
	}


	/// <summary>
	/// This method will be called on each frame <see cref="Replayable"/> recording. 
	/// <param name="dataIdx">Number of recording data</param>
	/// </summary>
	public void ReplayRecording (int dataIdx)
	{
	}

	/// <summary>
	/// This method will be called on each frame <see cref="Replayable"/> replaying. 
	/// <param name="dataIdx">Number of replaying data</param>
	/// </summary>
	public void ReplayPlaying (int dataIdx)
	{
	}


	/// <summary>
	/// This method will be called on each frame <see cref="Replayable"/> replaying. 
	/// <param name="t">Time for replay.</param>
	/// </summary>
	public void ReplayPlayingComplete (float t)
	{
	}
	
	
}
