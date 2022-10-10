namespace AnimExpress
{
	internal class AnimationEventChecker
	{
		public bool hasBeenTriggered;
		public float triggerTime;
		public AnimationExpressEvent animationEvent;

		public AnimationEventChecker(AnimationExpressEvent animationEvent)
		{
			this.animationEvent = animationEvent;
			this.hasBeenTriggered = false;
		}

		public void SetupTrigger(AnimationExpress animation)
		{
			triggerTime = animationEvent.TriggerTime * animation.TotalDuration;
		}
	}
}