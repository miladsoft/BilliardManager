(function($R)
{
    $R.add('plugin', 'article_links', {
        modals: {
            'article_links': ''
        },
        translations: {
            en: {
                "article_links": "مقالات",
                "article_links-label": "لطفا بر روی عنوان مقاله مورد نظر کلیک کنید"
            }
        },
        init: function(app)
        {
            // define app
            this.app = app;

            // define services
            this.lang = app.lang;
            this.toolbar = app.toolbar;
            this.insertion = app.insertion;
            this.opts = app.opts
        },

        // messages
        onmodal: {
            article_links: {
                open: function($modal)
                {
                    this._build($modal);
                },
            }
        },

        // public
        start: function()
        {
            if (ticketaArticleLinks) {
                var buttonData = {
                    title: this.lang.get('article_links'),
                    api: 'plugin.article_links.open'
                };

                // create the button
                var $button = this.toolbar.addButton('article_links', buttonData);
            }

        },
        open: function()
        {
            var options = {
                title: this.lang.get('article_links'),
                width: '500px',
                name: 'article_links',
            };

            this.app.api('module.modal.build', options);
        },
        // private
        _build: function($modal)
        {
            var $body = $modal.getBody();
            // var $label = this._buildLabel();
            var $list = this._buildList();

            this._buildItems($list);

            $body.html('');
            $body.append(this.lang.get('article_links-label'));
            $body.append($list);

        },

        _buildList: function()
        {
            var $list = $R.dom('<ul>');
            $list.addClass('redactor-modal-list');

            return $list;
        },
        _buildItems: function($list)
        {
            var items = ticketaArticleLinks;
            for (var i = 0; i < items.length; i++) {
                var li = $R.dom('<li>');
                if (items[i][0] == 'heading') {
                    var span = $('<span class="redactor-clip-heading">').text(items[i][1]);
                    li.append(span);
                } else {
                    var a = $('<a href="#" class="redactor-clip-link">').text(items[i][0]);
                    var div = $('<div class="redactor-clip">').hide().html(items[i][1]);
                    li.attr('data-index', i);

                    li.on('click', this._insert.bind(this));

                    li.append(a);
                    li.append(div);
                }

                $list.append(li);
            }

        },
        // private
        _insert: function(data)
        {

            data.preventDefault();
            var $item = $R.dom(data.target.nextSibling);
            this.insertion.insertRaw($item.html());
        }
    });
})(Redactor);