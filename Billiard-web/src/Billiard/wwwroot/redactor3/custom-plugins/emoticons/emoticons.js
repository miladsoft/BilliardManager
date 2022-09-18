(function ($R) {
    $R.add('plugin', 'emoticons',
        {

            init: function () {
                if (!this.opts.emoticons) {
                    this.opts.emoticons = [
                        ['&nbsp;:)', 'smile'],
                        [' :)', 'smile'],
                        [':smile:', 'smile'],
                        [':bigsmile:', 'bigsmile'],
                        ['&nbsp;:D', 'bigsmile'],
                        [' :D', 'bigsmile'],
                        [':happy:', 'bigsmile'],
                        [':frown:', 'frown'],
                        ['&nbsp;:(', 'frown'],
                        [' :(', 'frown'],
                        [':sad:', 'frown'],
                        [':angry:', 'angry'],
                        [':mad:', 'angry'],
                        [':love:', 'love'],
                        [':heart:', 'love'],
                        [':scared:', 'scared'],
                        [':silly:', 'silly'],
                        ['&nbsp;:P', 'silly'],
                        [' :P', 'silly'],
                        [':tongue:', 'silly'],
                        [':wink:', 'wink'],
                        ['&nbsp;;)', 'wink'],
                        [' ;)', 'wink'],
                        [':wow:', 'wow'],
                        [':surprised:', 'surprised'],
                        [':surprise:', 'surprised']
                    ];
                }

                this.$editor.on('keyup.redactor-plugin-emoticons', $.proxy(function (e) {
                    var key = e.which;
                    //if (key === this.keyCode.SPACE)
                    //{
                    var current = this.selection.current();
                    var cloned = $(current).clone();

                    var $div = $('<div>');
                    $div.html(cloned);

                    var text = $div.html();
                    $div.remove();

                    var len = this.opts.emoticons.length;
                    var replaced = 0;

                    for (var i = 0; i < len; i++) {

                        var smileyStr = (this.opts.emoticons[i][0] + '').replace(/([\\\.\+\*\?\[\^\]\$\(\)\{\}\=\!\<\>\|\:])/g, "\\$1");
                        var re = new RegExp('(' + smileyStr + ')', 'g');

                        if (text.search(re) !== -1) {

                            replaced++;
                            text = text.replace(re, '<img src="https://assets.ticketa.com/app/' + ticketa_public_theme + '/shared_assets/redactor2/emoticons/emoticons/' + this.opts.emoticons[i][1] + '.png" class="ticketa-emoticon" />');

                            $div = $('<div>');
                            $div.html(text);
                            $div.append(this.selection.marker());

                            var html = $div.html().replace(/&nbsp;/, ' ');

                            $(current).replaceWith(html);
                            $div.remove();
                        }
                    }

                    if (replaced !== 0) {
                        this.selection.restore();
                    }
                    //}


                }, this));

            }
        });
})(Redactor);