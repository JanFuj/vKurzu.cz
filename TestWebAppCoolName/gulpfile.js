'use strict';

var gulp                 = require('gulp');
var sass                 = require('gulp-sass');
var cleancss             = require('gulp-clean-css');
var rename               = require('gulp-rename');
var webserver            = require('gulp-webserver');
var debug                = require('gulp-debug');
var livereload           = require('gulp-livereload');
var errorHandler         = require('gulp-error-handle');
var autoprefixer         = require('gulp-autoprefixer');
var sourcemaps           = require('gulp-sourcemaps');

//var autoprefix           = new autoprefixer({ browsers: ["Chrome >= 57", "Edge >= 39", "Safari >= 10", "iOS >= 10", "Opera 50", "Firefox 50"] });
var Fiber                = require('fibers');
sass.compiler            = require('node-sass');

var path = {
    src: {
        scripts:            'Scripts/**/*.js',
        styles:             'styles/**/*.scss',
        styles_main:        'styles/*.scss',
        media:              'media/*.*',
        fonts:              'fonts/**/*.*'
    },
    dist: {
        scripts:            'pages/resources/scripts/',
        styles:             'pages/resources/styles/',
        media:              'pages/resources/media/',
        fonts:              'pages/resources/fonts/'
    }
};

gulp.task('watch', function() {
    gulp.watch(path.src.scripts,    { interval: 2000 }, gulp.series('scripts'));
    gulp.watch(path.src.media,      { interval: 2000 }, gulp.series('media'));
    gulp.watch(path.src.styles,     { interval: 500 },  gulp.series('styles'));
});

gulp.task('scripts', function() {
    return gulp.src(path.src.scripts)
        .pipe(errorHandler())
        .pipe(gulp.dest(path.dist.scripts))
        .pipe(livereload());
});

gulp.task('styles', function() {
    return gulp.src(path.src.styles_main)
        .pipe(errorHandler())
        .pipe(sourcemaps.init())
        //.pipe(cached('styles'))
        .pipe(debug({ title: 'SASS' }))
        .pipe(sass({fiber: Fiber}).on('error', sass.logError))
        .pipe(gulp.dest(path.dist.styles))
        .pipe(cleancss())
        .pipe(rename({
            suffix: '.min'
        }))
        //.pipe(remember('styles'))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(path.dist.styles))
        .pipe(livereload());
 });

gulp.task('media', function() {
    return gulp.src(path.src.media)
        .pipe(errorHandler())
        .pipe(gulp.dest(path.dist.media))
        .pipe(livereload());
});

gulp.task('fonts', function() {
    return gulp.src(path.src.fonts)
        .pipe(errorHandler())
        .pipe(gulp.dest(path.dist.fonts))
        .pipe(livereload());
});

gulp.task('setWatch', function(done) {
    global.isWatching = true;
    done();
});

gulp.task('webserver', function() {
    return gulp.src('./')
        .pipe(webserver({
            livereload: true,
            directoryListing: true,
            open: "/Pages/"
        }));
});
 

gulp.task('default', gulp.parallel('scripts', 'media', 'fonts' ,'styles', 'webserver' , 'watch'));
gulp.task('build', gulp.parallel('scripts', 'media', 'fonts' , 'styles'));
gulp.task('dev', gulp.parallel('setWatch', 'webserver' , 'watch'));
