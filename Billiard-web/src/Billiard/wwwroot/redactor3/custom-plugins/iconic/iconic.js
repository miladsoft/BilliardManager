(function ($R) {
	$R.add('plugin', 'iconic2',
	{

			init: function()
			{
				var icons = {
					'html': '<i class="ti ti-code"></i>',
					'clips': '<i class="ti ti-commenting"></i>&nbsp;&nbsp;پاسخ ها',
					'article_links': '<i class="ti ti-files-o"></i>&nbsp;&nbsp;مقالات',
					'format': '<i class="ti ti-format"></i>',
					'lists': '<i class="ti ti-ulist"></i>',
					'bold': '<i class="ti ti-bold"></i>',
					'italic': '<i class="ti ti-italic"></i>',
					'link': '<i class="ti ti-link"></i>',
					'horizontalrule': '<i class="ti ti-minus"></i>',
					'image': '<i class="ti ti-photo"></i>',
					'video': '<i class="ti ti-video"></i>',
					'file': '<i class="ti ti-paperclip"></i>',
					'table': '<i class="ti ti-table"></i>',
					'alignment': '<i class="ti ti-align-left"></i>'
				};


				$.each(this.button, $.proxy(function(i,s)
				{
					var key = $(s).attr('rel');

					if (typeof icons[key] !== 'undefined')
					{
						var icon = icons[key];
						var button = this.button.get(key);
						this.button.setIcon(button, icon);
					}

				}, this));

			},
		button : function(){
			return toolbar.getButtons();
		}

	});
})(Redactor);