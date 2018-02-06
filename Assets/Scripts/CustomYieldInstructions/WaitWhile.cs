using System.Collections;
using UnityEngine;
using System;

// Implementation of WaitWhile yield instruction. This can be later used as:
// yield return new WaitWhile(() => Princess.isInCastle);
class WaitWhile : CustomYieldInstruction
{
    Func<bool> m_Predicate;

    public override bool keepWaiting { get { return m_Predicate(); } }

    public WaitWhile(Func<bool> predicate) { m_Predicate = predicate; }
}