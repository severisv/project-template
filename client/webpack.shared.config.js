const { resolve, join } = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const autoprefixer = require('autoprefixer');

module.exports = {
  context: resolve(__dirname, 'src'),
  entry: ['./index.tsx'],
  resolve: {
    modules: [join(__dirname, 'src'), join(__dirname, 'static'), 'node_modules'],
    extensions: ['.js', '.ts', '.tsx', '.less'],
    enforceExtension: false,
    alias: { moment$: 'moment/moment.js' }
  },
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        loader: 'ts-loader',
        exclude: /node_modules/
      },
      {
        test: /\.(css|less)$/,
        use: ExtractTextPlugin.extract({
          fallback: 'style-loader',
          use: [
            {
              loader: 'css-loader',
              options: {
                convertValues: false,
                autoprefixer: false,
                sourceMap: true
              }
            },
            {
              loader: 'postcss-loader',
              options: {
                plugins: () => {
                  return [
                    autoprefixer({
                      browsers: ['> 1%', 'last 2 versions', 'Firefox ESR', 'IE 11']
                    })
                  ];
                }
              }
            },
            {
              loader: 'less-loader',
              options: {
                sourceMap: true,
                paths: [join(__dirname, 'static')]
              }
            }
          ]
        })
      },
      {
        test: /.*\.(jpe?g|png|gif)$/i,
        exclude: /(common)/,
        use: [
          {
            loader: 'file-loader',
            options: {
              hash: 'sha512',
              digest: 'hex',
              name: 'images/[name].[ext]',
            }
          }
        ]
      },
      {
        test: /\.(ttf|otf|eot|svg|woff(2)?)(\?[a-z0-9]+)?$/,
        exclude: /(common)/,
        use: [
          {
            loader: 'file-loader',
            options: {
              hash: 'sha512',
              digest: 'hex',
              name: 'fonts/[name].[ext]'
            }
          }
        ]
      }
    ]
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: '../Index-template.html',
      filename: join(__dirname, '..', 'server/wwwroot/Index.cshtml'),
      inject: 'both',
      hash: true
    })
  ]
};
