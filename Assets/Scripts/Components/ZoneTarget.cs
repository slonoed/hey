using System.Collections.Generic;
using Leopotam.Ecs;

namespace YANTH {
  public struct ZoneTarget : IEcsAutoReset<ZoneTarget> {
    public HashSet<string> tags;
    public HashSet<string> previousTags;

    public void AutoReset(ref ZoneTarget target) {
      target.tags = new HashSet<string>();
      target.previousTags = new HashSet<string>();
    }
  }
}