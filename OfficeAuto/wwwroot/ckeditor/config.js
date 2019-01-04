/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'contextmenu';
    config.extraPlugins = 'autolink';
    config.extraPlugins = 'textmatch';
    config.extraPlugins = 'panel';
    config.extraPlugins = 'floatpanel';
    config.extraPlugins = 'menu';
    config.extraPlugins = 'openlink';
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
    //Adding File Upload button manually.. #Raheel Code
    //config.filebrowserBrowseUrl = 'javascript:void(0)'; 
    //config.filebrowserUploadUrl = '/wwwroot/ReferenceDocs/';
};
