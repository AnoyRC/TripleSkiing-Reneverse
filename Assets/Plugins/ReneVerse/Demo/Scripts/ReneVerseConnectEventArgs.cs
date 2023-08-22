using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReneVerse.Demo
{
    [Serializable]
    public class ReneVerseConnectEventArgs : EventArgs
    {
        [SerializeField] private string _fullName;
        [SerializeField] private string _firstName;
        [SerializeField] private string _lastName;
        [SerializeField] private List<ReneAssetSample> _reneAssets;


        public List<ReneAssetSample> ReneAssets => _reneAssets;

        public string FullName => _fullName;

        public string FirstName => _firstName;

        public string LastName => _lastName;

        public ReneVerseConnectEventArgs(List<ReneAssetSample> reneAssets, string fullName)
        {
            _fullName = fullName;
            _reneAssets = reneAssets;
        }
    }
}