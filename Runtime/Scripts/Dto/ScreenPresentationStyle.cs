namespace QonversionUnity
{
    public enum ScreenPresentationStyle
    {
        /// on Android - default screen transaction animation will be used.
        /// on iOS - not a modal presentation. This style pushes a controller to a current navigation stack.
        /// For iOS NavigationController on the top of the stack is required.
        Push,

        /// on Android - screen will move from bottom to top.
        /// on iOS - UIModalPresentationFullScreen analog.
        FullScreen,

        /// iOS only - UIModalPresentationPopover analog
        Popover,

        /// Android only - screen will appear/disappear without any animation
		/// For iOS consider providing the <see cref="ScreenPresentationConfig.Animated"/> flag.
        NoAnimation,
    }
}
