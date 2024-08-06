import 'package:animated_background/animated_background.dart';
import 'package:flutter/material.dart';

class AnimatedHiveBackground extends StatefulWidget {
  const AnimatedHiveBackground({super.key, required this.child});
  final Widget child;

  @override
  State<AnimatedHiveBackground> createState() => _AnimatedHiveBackgroundState();
}

class _AnimatedHiveBackgroundState extends State<AnimatedHiveBackground>
    with TickerProviderStateMixin {
  @override
  Widget build(BuildContext context) {
    return AnimatedBackground(
        behaviour: RandomParticleBehaviour(
          options: const ParticleOptions(
            spawnMaxRadius: 100,
            spawnMinRadius: 20,
            spawnMinSpeed: 10.00,
            spawnMaxSpeed: 20,
            particleCount: 10,
            minOpacity: 0.4,
            maxOpacity: 1,
            spawnOpacity: 0.0,
            opacityChangeRate: 0.05,
            baseColor: Colors.blue,
            image: Image(
                image: NetworkImage(
                    'https://creazilla-store.fra1.digitaloceanspaces.com/icons/3429441/diamond-pentagon-20-17-yellow-icon-sm.png')),
          ),
        ),
        vsync: this,
        child: widget.child);
  }
}
