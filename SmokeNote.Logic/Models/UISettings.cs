using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmokeNote.Logic.Enums;

namespace SmokeNote.Logic.Models
{
    public class UISettings : SettingsBase
    {
        public UISettings()
        {
            this.ArticleDisplay = ArticleDisplays.Summary;
            this.ShowArticleEditToolbar = true;
            this.ShowArticleListPanel = true;
            this.ShowArticleProperties = true;
            this.ShowLeftPanel = true;
            this.ShowSearchDescription = true;

            this.LeftPanelMinWidth = 200;
            this.LeftPanelWidth = 200;
        }

        #region 属性

        private bool _showLeftPanel;

        /// <summary>
        /// 是否显示左侧面板
        /// </summary>
        public bool ShowLeftPanel
        {
            get { return _showLeftPanel; }
            set
            {
                if (_showLeftPanel != value)
                {
                    _showLeftPanel = value;
                    this.RaisePropertyChanged("ShowLeftPanel");
                }
            }
        }

        private bool _showArticleListPanel;

        /// <summary>
        /// 是否显示笔记列表面板
        /// </summary>
        public bool ShowArticleListPanel
        {
            get { return _showArticleListPanel; }
            set
            {
                if (_showArticleListPanel != value)
                {
                    _showArticleListPanel = value;
                    this.RaisePropertyChanged("ShowArticleListPanel");
                }
            }
        }

        private bool _showSearchDescription;

        /// <summary>
        /// 是否显示搜索说明
        /// </summary>
        public bool ShowSearchDescription
        {
            get { return _showSearchDescription; }
            set
            {
                if (_showSearchDescription != value)
                {
                    _showSearchDescription = value;
                    this.RaisePropertyChanged("ShowSearchDescription");
                }
            }
        }

        private bool _showArticleProperties;

        /// <summary>
        /// 是否显示笔记属性
        /// </summary>
        public bool ShowArticleProperties
        {
            get { return _showArticleProperties; }
            set
            {
                if (_showArticleProperties != value)
                {
                    _showArticleProperties = value;
                    this.RaisePropertyChanged("ShowArticleProperties");
                }
            }
        }

        private bool _showArticleEditToolbar;

        /// <summary>
        /// 是否显示笔记编辑工具条
        /// </summary>
        public bool ShowArticleEditToolbar
        {
            get { return _showArticleEditToolbar; }
            set
            {
                if (_showArticleEditToolbar != value)
                {
                    _showArticleEditToolbar = value;
                    this.RaisePropertyChanged("ShowArticleEditToolbar");
                }
            }
        }

        private ArticleDisplays _articleDisplay;

        /// <summary>
        /// 笔记显示方式
        /// </summary>
        public ArticleDisplays ArticleDisplay
        {
            get { return _articleDisplay; }
            set
            {
                if (_articleDisplay != value)
                {
                    _articleDisplay = value;
                    this.RaisePropertyChanged("ArticleDisplay");
                }
            }
        }

        private double _leftPanelMinWidth;

        /// <summary>
        /// 左侧面板最小宽度
        /// </summary>
        public double LeftPanelMinWidth
        {
            get { return _leftPanelMinWidth; }
            set
            {
                if (_leftPanelMinWidth != value)
                {
                    _leftPanelMinWidth = value;
                    this.RaisePropertyChanged("LeftPanelMinWidth");
                }
            }
        }

        private double _leftPanelWidth;

        /// <summary>
        /// 左侧面板宽度
        /// </summary>
        public double LeftPanelWidth
        {
            get { return _leftPanelWidth; }
            set
            {
                if (_leftPanelWidth != value)
                {
                    _leftPanelWidth = value;
                    this.RaisePropertyChanged("LeftPanelWidth");
                }
            }
        }

        #endregion
    }
}
