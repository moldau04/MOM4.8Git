CKEDITOR.plugins.add('preventdrop',
        {
        init: function (editor) {

            function onDragStart(event) {   
				alert('drop');	
                event.data.preventDefault(true); 
            };

            editor.on('contentDom', function (e) {                
                //editor.document.on('drop', onDragStart);
				
				var dragstart_outside = true;
				editor.document.on('dragstart', function (ev) {
				   dragstart_outside = false;
				});
				
				editor.document.on('drop', function (ev) {
				   if (dragstart_outside) {				   
					  ev.data.preventDefault(true);
				   }
				   dragstart_outside = true;
				});
				
            });						
        }
});