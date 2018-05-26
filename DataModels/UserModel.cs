﻿using System;
using System.Collections.Generic;

namespace DataModels
{
    [Serializable]
    public class UserModel
    {
        public UserType Type { get; set; }

        /// <summary>
        /// 아이디 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 닉네임
        /// </summary>
        public string Nic { get; set; }

        /// <summary>
        /// 포함된 bj정보
        /// </summary>
        public List<BjModel> BJs { get; set; }

        /// <summary>
        /// 사진url (BJ일경우)
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 아이콘 Url (하위 정보로 쓰일 때 사용) (BJ일경우?)
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 새로 추가된 요소 여부 (전부다 리프레쉬 해주니까 필요없음)
        /// </summary>
        //public bool IsNew { get; set; }

        /// <summary>
        /// 화면에 표시할 html (재사용을 위해 캐시)
        /// </summary>
        public string Html { get; set; }
    }

    public enum UserType
    {
        BJ,
        King,
        BigFan,
    }
}
