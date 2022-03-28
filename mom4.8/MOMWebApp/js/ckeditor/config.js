/*
Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';   
    CKEDITOR.config.enterMode = CKEDITOR.ENTER_BR;
    CKEDITOR.config.shiftEnterMode = CKEDITOR.ENTER_BR;
    CKEDITOR.on('instanceReady', function (ev) {
        var writer = ev.editor.dataProcessor.writer;
        // The character sequence to use for every indentation step.
        writer.indentationChars = '  ';

        var dtd = CKEDITOR.dtd;
        // Elements taken as an example are: block-level elements (div or p), list items (li, dd), and table elements (td, tbody).
        for (var e in CKEDITOR.tools.extend({}, dtd.$block, dtd.$listItem, dtd.$tableContent)) {
            ev.editor.dataProcessor.writer.setRules(e, {
                // Indicates that an element creates indentation on line breaks that it contains.
                indent: false,
                // Inserts a line break before a tag.
                breakBeforeOpen: false,
                // Inserts a line break after a tag.
                breakAfterOpen: false,
                // Inserts a line break before the closing tag.
                breakBeforeClose: false,
                // Inserts a line break after the closing tag.
                breakAfterClose: false
            });
        }

        for (var e1 in CKEDITOR.tools.extend({}, dtd.$list, dtd.$listItem, dtd.$tableContent)) {
            ev.editor.dataProcessor.writer.setRules(e1, {
                indent: true
            });
        }
    });
};
