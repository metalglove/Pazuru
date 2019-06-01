<template>
  <div id="app">
    <div id="nav">
      <router-link to="/">Home</router-link> |
      <router-link to="/puzzle">Puzzle</router-link> |
      <router-link to="/previouslysolvedpuzzles">Previously solved puzzles</router-link>
    </div>
    <router-view/>
    <Modal v-if="modalViewModel.showModal" @close="hideModal()"/>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { mapState, StoreOptions, mapGetters } from 'vuex';
import { RootState } from '@/store/RootState';
import { ModalViewModel } from './viewmodels/ModalViewModel';
import Modal from './components/Modal.vue';

@Component({
  components: {
    Modal
  },
  computed: {
    ...mapState(['modalViewModel'])
  }
})
export default class Wrapper extends Vue {
  private hideModal(): void {
    this.$store.state.modalViewModel.showModal = false;
  }
}
</script>

<style lang="scss">
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
#nav {
  padding: 30px;
  a {
    font-weight: bold;
    color: #2c3e50;
    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>
