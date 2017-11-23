const { resolve, join } = require('path');
const webpack = require('webpack');
const SharedConfig = require('./webpack.shared.config.js');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
  ...SharedConfig,
  entry: [
    'babel-polyfill',
    'react-hot-loader/patch',
    'webpack-dev-server/client?http://localhost:8080',
    'webpack/hot/only-dev-server',
    ...SharedConfig.entry
  ],
  output: {
    filename: 'bundle.js',
    path: resolve(__dirname, '../server/wwwroot'),
    publicPath: 'http://localhost:8080/'
  },

  devtool: 'inline-source-map',
  plugins: [
    ...SharedConfig.plugins,
    new ExtractTextPlugin({ disable: true }),
    new webpack.HotModuleReplacementPlugin(),
    new webpack.NamedModulesPlugin(),
    new webpack.DefinePlugin({
      __DEV__: true
    })
  ]
};
