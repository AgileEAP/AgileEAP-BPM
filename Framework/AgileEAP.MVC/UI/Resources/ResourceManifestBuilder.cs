using System.Collections.Generic;
using AgileEAP.Environment.Extensions.Models;

namespace AgileEAP.UI.Resources {
    public class ResourceManifestBuilder {
        public ResourceManifestBuilder() {
            ResourceManifests = new HashSet<IResourceManifest>();
        }

        public Feature Feature { get; set; }

        internal HashSet<IResourceManifest> ResourceManifests { get; private set; }

        public ResourceManifest Add() {
            var manifest = new ResourceManifest { Feature = Feature };
            ResourceManifests.Add(manifest);
            return manifest;
        }

        public void Add(IResourceManifest manifest) {
            ResourceManifests.Add(manifest);
        }
    }
}
